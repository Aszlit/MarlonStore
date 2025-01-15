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
        public ObservableCollection<Order> Orders { get; set; }
        private double _totalSubAmount;

        public PointSale()
        {
            InitializeComponent();
            Products = new ObservableCollection<Item>();
            Orders = new ObservableCollection<Order>();
            this.DataContext = this; // Set DataContext for binding
            LoadProducts(); // Load products from database
            OrdersPanel.ItemsSource = Orders; // Bind the orders to the ItemsControl
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
                orderDetails.OrderAdded += OnOrderAdded;

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

        private void OnOrderAdded(string name, int quantity, double price, BitmapImage productImage)
        {
            Orders.Add(new Order { Name = name, Quantity = quantity, Price = price, ProductImage = productImage });
            _totalSubAmount += quantity * price;
            TotalSubAmount.Text = $"Subtotal Amount: {_totalSubAmount:C}";
            OrdersPanel.ItemsSource = Orders;
        }

        private void ConfirmOrderButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var order in Orders)
            {
                var product = Products.FirstOrDefault(p => p.ItemName == order.Name);
                if (product != null)
                {
                    product.Quantity -= order.Quantity;
                }
            }

            // Update the database with the new quantities
            UpdateInventory();

            // Insert order details into Purchases table
            InsertOrderDetailsIntoPurchases();

            MessageBox.Show("Order confirmed and inventory updated!");

            // Clear orders and reset total sub amount
            Orders.Clear();
            _totalSubAmount = 0;
            TotalSubAmount.Text = $"Total Sub Amount: {_totalSubAmount:C}";
        }

        private void UpdateInventory()
        {
            // Get the relative path to the database
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var product in Products)
                {
                    var command = new SQLiteCommand("UPDATE Inventory SET Quantity = @Quantity WHERE ItemName = @ItemName", connection);
                    command.Parameters.AddWithValue("@Quantity", product.Quantity);
                    command.Parameters.AddWithValue("@ItemName", product.ItemName);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void InsertOrderDetailsIntoPurchases()
        {
            // Get the relative path to the database
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                foreach (var order in Orders)
                {
                    var command = new SQLiteCommand("INSERT INTO Purchases (ProductName, Amount, Quantity, TotalAmount, Date, Time) VALUES (@ProductName, @Amount, @Quantity, @TotalAmount, @Date, @Time)", connection);
                    command.Parameters.AddWithValue("@ProductName", order.Name);
                    command.Parameters.AddWithValue("@Amount", order.Price);
                    command.Parameters.AddWithValue("@Quantity", order.Quantity);
                    command.Parameters.AddWithValue("@TotalAmount", order.Total);
                    command.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@Time", DateTime.Now.ToString("HH:mm:ss"));
                    command.ExecuteNonQuery();
                }
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

        // Order model class
        public class Order
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public BitmapImage ProductImage { get; set; } // For displaying the image
            public double Total => Quantity * Price; // Calculated property for total
        }
    }
}
