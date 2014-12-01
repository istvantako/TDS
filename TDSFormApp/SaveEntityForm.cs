using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestDataSeeding.Logic;
using TestDataSeeding.SerializedStorage;
using TestDataSeeding.Model;

namespace TDSFormApp
{
    public partial class SaveEntityForm : Form
    {
        //IStorage xml = new XmlStorageClient();
        XmlStorageClient xml = new XmlStorageClient();
        EntityStructures entityStructures = new EntityStructures();
        EntityStructure entityStructure = new EntityStructure();

        public SaveEntityForm()
        {
            InitializeComponent();
        }

        private void SaveEntityForm_Load(object sender, EventArgs e)
        {
            entityStructures = xml.GetEntityStructures("d:\\TDS\\");
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
            Label[] pk_label = new Label[n];
            TextBox[] pk_textbox = new TextBox[n];

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
        }
    }
}
