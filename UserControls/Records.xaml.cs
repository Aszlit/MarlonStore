using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
using System.Windows.Controls.Primitives;

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for Records.xaml
    /// </summary>
    public partial class Records : UserControl
    {
        public Records()
        {
            InitializeComponent();
        }

        private void InventoryButton_Click(object sender, RoutedEventArgs e)
        {
            LoadInventoryData();
        }

        private void PurchasesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPurchasesData();
        }

        private void SuppliersButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliersData();
        }

        private void LoadInventoryData()
        {
            try
            {
                var items = LoadInventory();

                RecordsDataGrid.Columns.Clear();
                var imageColumn = new DataGridTemplateColumn
                {
                    Header = "Image",
                    Width = 60,
                    CellTemplate = new DataTemplate
                    {
                        VisualTree = new FrameworkElementFactory(typeof(Image))
                    }
                };

                var imageFactory = (FrameworkElementFactory)imageColumn.CellTemplate.VisualTree;
                imageFactory.SetBinding(Image.SourceProperty, new Binding("ProductImage"));
                imageFactory.SetValue(Image.WidthProperty, 50.0);
                imageFactory.SetValue(Image.HeightProperty, 50.0);
                imageFactory.SetValue(Image.StretchProperty, Stretch.UniformToFill);
                imageFactory.SetValue(Image.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                RecordsDataGrid.Columns.Add(imageColumn);
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Item Name",
                    Binding = new Binding("ItemName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Quantity",
                    Binding = new Binding("Quantity"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Price",
                    Binding = new Binding("Price"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Value",
                    Binding = new Binding("Value"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });

                RecordsDataGrid.ItemsSource = items;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading inventory data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPurchasesData()
        {
            try
            {
                var purchases = LoadPurchases();

                RecordsDataGrid.Columns.Clear();
                var imageColumn = new DataGridTemplateColumn
                {
                    Header = "Image",
                    Width = 60,
                    CellTemplate = new DataTemplate
                    {
                        VisualTree = new FrameworkElementFactory(typeof(Image))
                    }
                };

                var imageFactory = (FrameworkElementFactory)imageColumn.CellTemplate.VisualTree;
                imageFactory.SetBinding(Image.SourceProperty, new Binding("ProductImage"));
                imageFactory.SetValue(Image.WidthProperty, 50.0);
                imageFactory.SetValue(Image.HeightProperty, 50.0);
                imageFactory.SetValue(Image.StretchProperty, Stretch.UniformToFill);
                imageFactory.SetValue(Image.HorizontalAlignmentProperty, HorizontalAlignment.Center);

                RecordsDataGrid.Columns.Add(imageColumn);
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Product Name",
                    Binding = new Binding("ProductName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Amount",
                    Binding = new Binding("Amount"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Quantity",
                    Binding = new Binding("Quantity"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Total Amount",
                    Binding = new Binding("TotalAmount"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Date",
                    Binding = new Binding("Date"),
                    Width = 150,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Time",
                    Binding = new Binding("Time"),
                    Width = 150,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });

                RecordsDataGrid.ItemsSource = purchases;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading purchases data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadSuppliersData()
        {
            try
            {
                var suppliers = LoadSuppliers();

                RecordsDataGrid.Columns.Clear();
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Supplier Name",
                    Binding = new Binding("SupplierName"),
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Contact",
                    Binding = new Binding("Contact"),
                    Width = 150,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Address",
                    Binding = new Binding("Address"),
                    Width = 200,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Phone Number",
                    Binding = new Binding("PhoneNumber"),
                    Width = 150,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Status",
                    Binding = new Binding("Status"),
                    Width = 100,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = "Date Added",
                    Binding = new Binding("DateAdded"),
                    Width = 150,
                    ElementStyle = (Style)FindResource("CenteredTextStyle")
                });

                RecordsDataGrid.ItemsSource = suppliers;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading suppliers data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == InventoryButton)
            {
                PurchasesButton.IsChecked = false;
                SuppliersButton.IsChecked = false;
                InventoryButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4E00A0")); // Highlight color
            }
            else if (sender == PurchasesButton)
            {
                InventoryButton.IsChecked = false;
                SuppliersButton.IsChecked = false;
                PurchasesButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4E00A0")); // Highlight color
            }
            else if (sender == SuppliersButton)
            {
                InventoryButton.IsChecked = false;
                PurchasesButton.IsChecked = false;
                SuppliersButton.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4E00A0")); // Highlight color
            }
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button != null)
            {
                button.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6102C4")); // Original color
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string searchText = SearchTextBox.Text.Trim().ToLower();

            try
            {
                if (InventoryButton.IsChecked == true)
                {
                    var filteredItems = LoadInventory().Where(item => item.ItemName.ToLower().Contains(searchText)).ToList();
                    RecordsDataGrid.ItemsSource = filteredItems;
                }
                else if (PurchasesButton.IsChecked == true)
                {
                    var filteredPurchases = LoadPurchases().Where(purchase => purchase.ProductName.ToLower().Contains(searchText)).ToList();
                    RecordsDataGrid.ItemsSource = filteredPurchases;
                }
                else if (SuppliersButton.IsChecked == true)
                {
                    var filteredSuppliers = LoadSuppliers().Where(supplier => supplier.SupplierName.ToLower().Contains(searchText)).ToList();
                    RecordsDataGrid.ItemsSource = filteredSuppliers;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while performing search: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<Item> LoadInventory()
        {
            var items = new List<Item>();

            try
            {
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
                            byte[]? imageBytes = reader["Image"] as byte[];
                            BitmapImage? image = null;

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

                            items.Add(new Item
                            {
                                ItemName = reader["ItemName"]?.ToString() ?? string.Empty,
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Price = Convert.ToDouble(reader["Price"]),
                                Value = Convert.ToDouble(reader["Value"]),
                                ProductImage = image ?? new BitmapImage()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading inventory: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return items;
        }

        private List<Purchase> LoadPurchases()
        {
            var purchases = new List<Purchase>();

            try
            {
                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand(@"
                                                    SELECT p.ProductName, p.Amount, p.Quantity, p.TotalAmount, p.Date, p.Time, i.Image
                                                    FROM Purchases p
                                                    JOIN Inventory i ON p.ProductName = i.ItemName", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            byte[]? imageBytes = reader["Image"] as byte[];
                            BitmapImage? image = null;

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

                            purchases.Add(new Purchase
                            {
                                ProductName = reader.GetString(0),
                                Amount = reader.GetDouble(1),
                                Quantity = reader.GetInt32(2),
                                TotalAmount = reader.GetDouble(3),
                                Date = reader.GetString(4),
                                Time = DateTime.Parse(reader.GetString(5)).ToString("hh:mm tt"),
                                ProductImage = image ?? new BitmapImage()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading purchases: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return purchases;
        }

        private List<Supplier> LoadSuppliers()
        {
            var suppliers = new List<Supplier>();

            try
            {
                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT supplier_name, contact, address, phone_number, status, date_added FROM Suppliers", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suppliers.Add(new Supplier
                            {
                                SupplierName = reader["supplier_name"]?.ToString() ?? string.Empty,
                                Contact = reader["contact"]?.ToString() ?? string.Empty,
                                Address = reader["address"]?.ToString() ?? string.Empty,
                                PhoneNumber = reader["phone_number"]?.ToString() ?? string.Empty,
                                Status = reader["status"]?.ToString() ?? string.Empty,
                                DateAdded = reader["date_added"] != DBNull.Value ? Convert.ToDateTime(reader["date_added"]) : DateTime.MinValue
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading suppliers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return suppliers;
        }

        public class Purchase
        {
            public string ProductName { get; set; }
            public double Amount { get; set; }
            public int Quantity { get; set; }
            public double TotalAmount { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public BitmapImage ProductImage { get; set; }
        }

        public class Supplier
        {
            public string SupplierName { get; set; }
            public string Contact { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
            public string Status { get; set; }
            public DateTime DateAdded { get; set; }
        }
    }
}
