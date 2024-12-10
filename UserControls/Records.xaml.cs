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

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for Purchases.xaml
    /// </summary>
    public partial class Records : UserControl
    {
        public Records()
        {
            InitializeComponent();
            LoadPurchases(); // Load inventory items from database
        }


        public class Purchase
        {
            public int PurchaseID { get; set; }
            public string ProductName { get; set; }
            public string SKU { get; set; }
            public int InboundStock { get; set; }
            public DateTime DateOrdered { get; set; }
            public DateTime? DateExpected { get; set; }
            public string PurchaseOrder { get; set; }
        }



        private void LoadPurchases()
        {
            using (var connection = new SQLiteConnection(@"Data Source=C:\Users\marlo\source\repos\Aszlit\MarlonStore\database\maindatabase.db"))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Purchases", connection);
                var reader = command.ExecuteReader();
                var purchases = new List<Purchase>();
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
                PurchasesDataGrid.ItemsSource = purchases;
            }
        }

        private void AddPurchase_Click(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) ||
                string.IsNullOrWhiteSpace(SKUTextBox.Text) ||
                string.IsNullOrWhiteSpace(InboundStockTextBox.Text) ||
                !int.TryParse(InboundStockTextBox.Text, out int inboundStock) ||
                DateOrderedPicker.SelectedDate == null ||
                DateExpectedPicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(PurchaseOrderTextBox.Text))
            {
                MessageBox.Show("Please fill in all fields correctly.", "Input Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=C:\Users\marlo\source\repos\Aszlit\MarlonStore\database\maindatabase.db"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("INSERT INTO Purchases (ProductName, SKU, InboundStock, DateOrdered, DateExpected, PurchaseOrder) " +
                                                    "VALUES (@ProductName, @SKU, @InboundStock, @DateOrdered, @DateExpected, @PurchaseOrder)", connection);

                    command.Parameters.AddWithValue("@ProductName", ProductNameTextBox.Text);
                    command.Parameters.AddWithValue("@SKU", SKUTextBox.Text);
                    command.Parameters.AddWithValue("@InboundStock", inboundStock);
                    command.Parameters.AddWithValue("@DateOrdered", DateOrderedPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@DateExpected", DateExpectedPicker.SelectedDate.Value.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@PurchaseOrder", PurchaseOrderTextBox.Text);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Purchase added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadPurchases(); // Refresh the DataGrid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == "Product Name" || textBox.Text == "SKU" ||
                    textBox.Text == "Inbound Stock" || textBox.Text == "P.O.")
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.Black;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    if (textBox == ProductNameTextBox) textBox.Text = "Product Name";
                    else if (textBox == SKUTextBox) textBox.Text = "SKU";
                    else if (textBox == InboundStockTextBox) textBox.Text = "Inbound Stock";
                    else if (textBox == PurchaseOrderTextBox) textBox.Text = "P.O.";

                    textBox.Foreground = Brushes.Gray;
                }
            }
        }
    }
}
