using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDataSeeding.Logic;
using TestDataSeeding.SerializedStorage;
using TestDataSeeding.Model;
using TestDataSeeding.Client;

namespace TDSFormApp
{
    public partial class SaveEntityForm : Form
    {
        private static IStorage xml = new XmlStorageClient();
        private static EntityStructures entityStructures = new EntityStructures();
        private static EntityStructure entityStructure = new EntityStructure();
        private static EntityWithKey entity;

        Label[] pk_label;
        TextBox[] pk_textbox;

        public SaveEntityForm()
        {
            InitializeComponent();
        }

        private void SaveEntityForm_Load(object sender, EventArgs e)
        {
            entityStructures = xml.GetEntityStructures("d:\\TDS\\");
            //entityCombobox.ViewColumn = 2;        be kene allitani, hogy tobb sor legyen
            foreach (EntityStructure entity in entityStructures)
            {
                entityCombobox.Items.Add(entity.Name);
            }
        }

        private void entityCombobox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.panel1.Controls.Clear();
            entityStructure = entityStructures.Find(entityCombobox.SelectedItem.ToString());

            int n = entityStructure.PrimaryKeys.Count;
            pk_label = new Label[n];
            pk_textbox = new TextBox[n];

            for (int i = 0; i < n; i++)
            {
                pk_label[i] = new Label();
                pk_textbox[i] = new TextBox();
                
            }
            int k = 0;
            foreach (String pk_name in entityStructure.PrimaryKeys)
            {
                pk_label[k].Text = pk_name;
                pk_label[k].Name = pk_name + "Label: ";
                pk_textbox[k].Name = pk_name + "Textbox";
                k++;
            }
            for (int i = 0; i < n; i++)
            {
                pk_label[i].Location = new Point(panel1.Left, panel1.Top + k);
                this.panel1.Controls.Add(pk_label[i]);
                //this.panel1.Controls.Add(new LiteralControl("<br />"));
                pk_textbox[i].Location = new Point(panel1.Left + pk_label[i].Width, panel1.Top + k);
                this.panel1.Controls.Add(pk_textbox[i]);
                k += 30;
            }
            //saveButton.Enabled = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            String path = "d:\\TDS\\";
            TdsClient tdsClient = new TdsClient(path);

            List<EntityWithKey> entities = new List<EntityWithKey>();
            List<string> parameters = new List<string>();
            string entityName;


            entityName = entityCombobox.SelectedItem.ToString();

            for (var i = 0; i < pk_textbox.Length; i++)
            {
                parameters.Add(pk_textbox[i].Text);
                Console.WriteLine(pk_textbox[i]);
            }
            if (parameters.Any())
            {
                entity = new EntityWithKey(entityName, parameters);
            }

            entities.Add(entity);

            try
            {
                tdsClient.SaveEntity(entities);
                MessageBox.Show("The given entity is saved.");
            }
            catch (Exception ex)
            {
                if (ex is EntityAlreadySavedException)
                    MessageBox.Show("The entity with the given keys has already been saved.");
                else
                    MessageBox.Show(ex.ToString());         //ezt dobja ki
            }
        }
    }
}
