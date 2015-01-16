using System.Linq;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using TestDataSeeding.Client;
using TestDataSeeding.Model;
using TestDataSeeding.Logic;
using TestDataSeeding.SerializedStorage;
using System.Configuration;
using System.Diagnostics;

namespace TdsWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TdsClient tdsClient = new TdsClient(ConfigurationManager.AppSettings["TdsStoragePath"]);
        private EntityStructures structures;
        private List<PrimaryKey> primaryKeys = new List<PrimaryKey>();

        public MainWindow()
        {
            InitializeComponent();

            LoadData();

            List<String> entities = new List<string>();
            foreach (var entityStructure in structures.Structures)
            {
                entities.Add(entityStructure.Name);
            }
            entities.Sort();
            ComboBoxEntities.ItemsSource = entities;  
        }

        private void ComboBoxEntities_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            primaryKeys.Clear();
            EntityStructure structure = structures.Find(ComboBoxEntities.SelectedValue.ToString());

            foreach (var attributeName in structure.PrimaryKeys)
            {
                primaryKeys.Add(new PrimaryKey(attributeName, "", structure.Attributes[attributeName]));
            }

            ItemsControlPrimaryKeys.ItemsSource = primaryKeys;
            ItemsControlPrimaryKeys.Items.Refresh();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            List<EntityWithKey> entities = new List<EntityWithKey>();
            List<string> parameters = new List<string>();

            foreach (var primaryKey in primaryKeys)
            {
                parameters.Add(primaryKey.Value);
            }

            if (parameters.Any())
            {
                entities.Add(new EntityWithKey(ComboBoxEntities.SelectedValue.ToString(), parameters));

                SaveEntity(entities);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();   
        }

        private void ButtonGenerateStructure_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                tdsClient.GenerateDatabaseStructure();
            }
            catch (Exception ex)
            {
                if (ex is EntityStructureAlreadyExistsException)
                {
                    MessageBoxResult result = MessageBox.Show("An entityStructure already exists.\n" +
                            "Proceed?", "GenerateStructure", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            tdsClient.GenerateDatabaseStructure();
                        }
                        catch (Exception exc)
                        {
                            MessageBox.Show("An exception occured:\n"+exc.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("An exception occured:\n" + ex.Message);
                }
            }
        }

        private void SaveEntity(List<EntityWithKey> entities)
        {
            try
            {
                tdsClient.SaveEntities(entities, overwrite: false);
                MessageBox.Show("The given entity is saved.");
            }
            catch (EntityAlreadySavedException)
            {
                MessageBoxResult result = MessageBox.Show("The entity with the given keys has already been saved.\nOverwrite?",
                                    "Already saved", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    tdsClient.SaveEntities(entities, overwrite: true);
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

        private void LoadData()
        {
            try
            {
                structures = tdsClient.GetEntityStructures();
            }
            catch (Exception e)
            {
                if (e is System.IO.FileNotFoundException)
                {
                    MessageBoxResult result = MessageBox.Show("There was no structure found.\nGenerate now?",
                                    "Missing Structure", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        tdsClient.GenerateDatabaseStructure();
                        structures = tdsClient.GetEntityStructures();
                        MessageBox.Show("Structure successfully generated.");
                    }
                    else
                    {
                        MessageBox.Show("The Entitysaver can`t operate without a Structure file, and it`s going to terminate.");
                        Environment.Exit(1);
                        //Application.Current.Shutdown(); 
                    }
                }
                else
                {
                    MessageBox.Show("An exception occured:\n" + e.Message);
                }
            }
        }
    }
}
