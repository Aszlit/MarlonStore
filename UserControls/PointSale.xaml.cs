using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Input;
using System.IO;




namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for PointSale.xaml
    /// </summary>
    public partial class PointSale : UserControl
    {
        public ObservableCollection<Item> Products { get; set; }

        public PointSale()
        {
            InitializeComponent();
            Products = new ObservableCollection<Item>();
            this.DataContext = this; // Set DataContext for binding
            LoadProducts(); // Load products from database
        }

        // Method to load data from SQLite database
        public void LoadProducts()
        {
            Products.Clear();

            // Get the relative path to the database
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
                            using (var stream = new MemoryStream(imageBytes))
                            {
                                image = new BitmapImage();
                                image.BeginInit();
                                image.StreamSource = stream;
                                image.CacheOption = BitmapCacheOption.OnLoad;
                                image.EndInit();
                            }
                        }

                        Products.Add(new Item
                        {
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDouble(reader["Price"]),
                            ProductImage = image // Assign the image to the property
                        });
                    }
                }
            }

            ProductsPanel.ItemsSource = Products; // Bind the products to the ItemsControl
        }

        private void Card_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = (sender as FrameworkElement)?.DataContext as Item;
            if (clickedItem != null)
            {
                var orderDetails = new OrderDetails();
                orderDetails.SetProductDetails(clickedItem.ProductImage, clickedItem.ItemName, clickedItem.Quantity, clickedItem.Price);

                var window = new Window
                {
                    Content = orderDetails,
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Title = "Order Details"
                };
                window.ShowDialog();
            }
        }

        // Item model class
        public class Item
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public BitmapImage ProductImage { get; set; } // For displaying the image
        }
    }
}
