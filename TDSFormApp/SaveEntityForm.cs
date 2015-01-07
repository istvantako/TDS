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
using TestDataSeeding.Model;
using TestDataSeeding.Client;
using System.Data;
using TestDataSeeding.SerializedStorage;

namespace TDSFormApp
{
    public partial class SaveEntityForm : Form
    {
        private static EntityStructures entityStructures = new EntityStructures();
        private static EntityStructure entityStructure = new EntityStructure();
        private static EntityWithKey entity;
        private static String path = ConfigurationManager.AppSettings["TdsStoragePath"];
        private TdsClient tdsClient = new TdsClient(path);
        private bool[] changed;
        Label[] pk_label;
        TDSFormTextbox[] pk_textbox;
        ToolTip[] pk_tooltip;
        //ToolTip saveButtonToolTip;
        public SaveEntityForm()
        {
            InitializeComponent();
        }

        private void SaveEntityForm_Load(object sender, EventArgs e)
        {
            entityStructures = tdsClient.GetEntityStructures();
            
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

            CreatePanelElements(n);

            LoadPanelElements();
            
            AddPanelElementsToPanel(n);
        }

        private void AddPanelElementsToPanel(int n)
        {
            int k = n;
            for (int i = 0; i < n; i++)
            {
                pk_label[i].Location = new Point(panel1.Left, panel1.Top + k);
                this.panel1.Controls.Add(pk_label[i]);
                pk_textbox[i].Location = new Point(panel1.Left + pk_label[i].Width, panel1.Top + k);
                pk_textbox[i].Width += 100;
                this.panel1.Controls.Add(pk_textbox[i]);
                k += 30;
            }
        }

        private void LoadPanelElements()
        {
            int k = 0;

            foreach (String pk_name in entityStructure.PrimaryKeys)
            {
                pk_label[k].Text = pk_name;
                pk_textbox[k].Name = k + "";
                pk_textbox[k].TextChanged += new EventHandler(textBox_TextChanged);
                pk_textbox[k].type = entityStructure.Attributes[pk_name];
              
                pk_tooltip[k].SetToolTip(this.pk_textbox[k], pk_textbox[k].type);
                k++;
            }
        }

        private void CreatePanelElements(int n)
        {
            pk_label = new Label[n];
            pk_textbox = new TDSFormTextbox[n];
            changed = new bool[n];
            pk_tooltip = new ToolTip[n];
            for (int i = 0; i < n; i++)
            {
                pk_label[i] = new Label();
                pk_textbox[i] = new TDSFormTextbox();
                pk_tooltip[i] = new ToolTip();
                changed[i] = new bool();
                changed[i] = false;
            }
        }
       
       
        private void textBox_TextChanged(object sender, EventArgs e)
        {
            int id = Int32.Parse(((TextBox)sender).Name);               //name of the primary key textbox
            pk_textbox[id].Validate();
            if (((TextBox)sender).Text != "")
            {
              
                    changed[id] = true;
          
            }
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

            SaveEntity(entities);
        }

        private void SaveEntity(List<EntityWithKey> entities)
        {
            try
            {
                tdsClient.SaveEntities(entities, overwrite_CheckBox.Checked);
                MessageBox.Show("The given entity is saved.");
            }
            catch (EntityAlreadySavedException)
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void generateStructuresButton_Click(object sender, EventArgs e)
        {
            try
            {
                
                DialogResult result = MessageBox.Show("If the database structure existed, it would be overwritten .\nOverwrite?",
                                    "Already save",
                                    MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    try
                    {
                        tdsClient.GenerateDatabaseStructure();
                    }
                    catch (EntityStructureAlreadyExistsException exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                    catch (Exception exc)
                    {
                        throw exc;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
