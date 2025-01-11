using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : UserControl
    {
        // ObservableCollection that binds to the DataGrid
        public ObservableCollection<Item> Items { get; set; }

        public Inventory()
        {
            InitializeComponent();
            Items = new ObservableCollection<Item>();
            this.DataContext = this; // Set DataContext for binding
            LoadInventory(); // Load inventory items from database
        }

        // Method to load data from SQLite database
        public void LoadInventory()
        {
            Items.Clear();

            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Inventory", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] imageBytes = reader["Image"] as byte[]; // Retrieve image data as byte array
                        BitmapImage image = null;

                        if (imageBytes != null)
                        {
                            using (var stream = new System.IO.MemoryStream(imageBytes))
                            {
                                image = new BitmapImage();
                                image.BeginInit();
                                image.StreamSource = stream;
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.EndInit();
                            }
                        }

                        Items.Add(new Item
                        {
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDouble(reader["Price"]),
                            Value = Convert.ToDouble(reader["Value"]),
                            ProductImage = image // Assign the image to the property
                        });
                    }
                }
            }
        }




        // Event handler for the "Add New Item" button click
        private void addbutton(object sender, RoutedEventArgs e)
        {
            try
            {
                // Create the new window
                AddNewItemWindow addgoodswindow = new AddNewItemWindow();

                // Set the owner of the new window to the current Inventory window
                addgoodswindow.Owner = Window.GetWindow(this);  // This sets the owner to the Inventory window

                // Show the new window and wait until it's closed
                addgoodswindow.ShowDialog();

                // Refresh the inventory after the new item is added
                LoadInventory(); // Refresh inventory after adding a new item
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }





        // Item model class
        public class Item
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public double Value { get; set; }
            public BitmapImage ProductImage { get; set; } // For displaying the image
        }



        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                // Your closing logic here
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during closing: {ex.Message}");
            }
        }

    }
}
