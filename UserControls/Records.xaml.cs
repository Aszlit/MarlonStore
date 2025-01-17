﻿using System;
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
            LoadInventory();
        }

        private void PurchasesButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPurchases();
        }

        private void LoadInventory()
        {
            var items = new List<Inventory.Item>();

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

                        items.Add(new Inventory.Item
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


        private void LoadPurchases()
        {
            var purchases = new List<Purchase>();

            using (var connection = new SQLiteConnection(@"Data Source=C:\Users\marlo\source\repos\Aszlit\MarlonStore\database\maindatabase.db"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Purchases", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        purchases.Add(new Purchase
                        {
                            PurchaseID = reader.GetInt32(0),
                            ProductName = reader.GetString(1),
                            SKU = reader.GetString(2),
                            InboundStock = reader.GetInt32(3),
                            DateOrdered = DateTime.Parse(reader.GetString(4)),
                            DateExpected = string.IsNullOrEmpty(reader.GetString(5)) ? (DateTime?)null : DateTime.Parse(reader.GetString(5)),
                            PurchaseOrder = reader.GetString(6)
                        });
                    }
                }
            }

            RecordsDataGrid.Columns.Clear();
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Product Name",
                Binding = new Binding("ProductName"),
                Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "SKU",
                Binding = new Binding("SKU"),
                Width = 100,
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Inbound Stock",
                Binding = new Binding("InboundStock"),
                Width = 100,
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Date Ordered",
                Binding = new Binding("DateOrdered"),
                Width = 150,
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "Date Expected",
                Binding = new Binding("DateExpected"),
                Width = 150,
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });
            RecordsDataGrid.Columns.Add(new DataGridTextColumn
            {
                Header = "P.O.",
                Binding = new Binding("PurchaseOrder"),
                Width = 100,
                ElementStyle = (Style)FindResource("CenteredTextStyle")
            });

            RecordsDataGrid.ItemsSource = purchases;
        }

        public class Purchase
        {
            public int PurchaseID { get; set; }
            public string ProductName { get; set; } = string.Empty; // Initialize with a default value
            public string SKU { get; set; } = string.Empty; // Initialize with a default value
            public int InboundStock { get; set; }
            public DateTime DateOrdered { get; set; }
            public DateTime? DateExpected { get; set; }
            public string PurchaseOrder { get; set; } = string.Empty; // Initialize with a default value
        }
    }
}
