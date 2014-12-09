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
//using TestDataSeeding.SerializedStorage;
using TestDataSeeding.Model;
using TestDataSeeding.Client;
using System.Data;

namespace TDSFormApp
{
    public partial class SaveEntityForm : Form
    {
        private static EntityStructures entityStructures = new EntityStructures();
        private static EntityStructure entityStructure = new EntityStructure();
        private static EntityWithKey entity;
        private static String path = "d:\\TDS\\";
        private TdsClient tdsClient = new TdsClient(path);
        private bool[] changed;
        Label[] pk_label;
        TextBox[] pk_textbox;

        public SaveEntityForm()
        {
            InitializeComponent();
        }

        private void SaveEntityForm_Load(object sender, EventArgs e)
        {
            entityStructures = tdsClient.GetEntityStructures();
            //entityCombobox.ViewColumn = 2;        be kene allitani, hogy tobb sor legyen
            
            foreach (EntityStructure entity in entityStructures.Structures)
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
            changed = new bool[n];

            //initialization
            for (int i = 0; i < n; i++)
            {
                pk_label[i] = new Label();
                pk_textbox[i] = new TextBox();
                changed[i] = new bool();
                changed[i] = false;
            }

            int k = 0;
            foreach (String pk_name in entityStructure.PrimaryKeys)
            {
                pk_label[k].Text = pk_name;
                //pk_label[k].Name = k + "Label: ";
                pk_textbox[k].Name = k + "";
                pk_textbox[k].TextChanged += new EventHandler(textBox_TextChanged);
                k++;
            }
            for (int i = 0; i < n; i++)
            {
                pk_label[i].Location = new Point(panel1.Left, panel1.Top + k);
                this.panel1.Controls.Add(pk_label[i]);
                pk_textbox[i].Location = new Point(panel1.Left + pk_label[i].Width, panel1.Top + k);
                this.panel1.Controls.Add(pk_textbox[i]);
                k += 30;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            int id = Int32.Parse(((TextBox)sender).Name);               //name of the primary key textbox
            if (((TextBox)sender).Text != "")
                changed[id] = true;
            else
            {
                saveButton.Enabled = false;
                changed[id] = false;
            }
            for (int i = 0; i < changed.Length; i++)
            {
                if (changed[i] != true)
                {
                    saveButton.Enabled = false;
                    return;
                }
            }
            saveButton.Enabled = true;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            List<string> parameters = new List<string>();
            string entityName;

            entityName = entityCombobox.SelectedItem.ToString();

            for (var i = 0; i < pk_textbox.Length; i++)
            {
                parameters.Add(pk_textbox[i].Text);
            }
            if (parameters.Any())
            {
                entity = new EntityWithKey(entityName, parameters);
            }

            entities.Add(entity);

            try
            {
                tdsClient.SaveEntities(entities);
                MessageBox.Show("The given entity is saved.");
            }
            catch (Exception ex)
            {
                if (ex is EntityAlreadySavedException)
                {
                    DialogResult result = MessageBox.Show("The entity with the given keys has already been saved.\nOverwrite?",
                                        "Already save",
                                        MessageBoxButtons.YesNo);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        tdsClient.SaveEntities(entities, true);
                        MessageBox.Show("The given entity is saved.");
                    }
                    else
                    {
                        MessageBox.Show("Save aborted.");
                        return;
                    }
                }
                else
                    MessageBox.Show(ex.Message);         
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fillCombobox(string str)
        {

            //foreach (EntityStructure entity in entityStructures.Structures)
            //{
            //    if (entity.Name.StartsWith(str))
            //    {
            //        entityCombobox.Items.Add(entity.Name);
            //        Console.WriteLine("string: " + str);
            //        Console.WriteLine("entity name: " + entity.Name);
            //    }
            //}
        }

        private void entityCombobox_TextChanged(object sender, EventArgs e)             
        {
            entityCombobox.Items.Clear();
           // string str = entityCombobox.Text;
            string str = "Table";                                                   //nincs kesz
            foreach (EntityStructure entity in entityStructures.Structures)
            {

                if (entity.Name.StartsWith(str))
                {
                    entityCombobox.Items.Add(entity.Name);
                    Console.WriteLine("string: " + str);
                    Console.WriteLine("entity name: " + entity.Name);
                }
            }
            //fillCombobox(entityCombobox.Text);
        }
    }
}
