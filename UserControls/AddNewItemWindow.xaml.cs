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
using System.Windows.Shapes;

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for AddNewItemWindow.xaml
    /// </summary>
    public partial class AddNewItemWindow : Window
    {
        public AddNewItemWindow()
        {
            InitializeComponent();
        }
        // Calculate Value Button Click
        private void CalculateValue_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var quantity = int.Parse(QuantityTextBox.Text);
                var price = double.Parse(PriceTextBox.Text);
                ValueTextBox.Text = (quantity * price).ToString("F2");
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter valid numbers for Quantity and Price.");
            }
        }

        



        // Convert image to byte array
        private byte[] ConvertImageToByteArray(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return System.IO.File.ReadAllBytes(imagePath);
        }



        // Save Button Click
        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var itemName = ItemNameTextBox.Text;
            var quantityText = QuantityTextBox.Text;
            var priceText = PriceTextBox.Text;
            var valueText = ValueTextBox.Text;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(itemName) ||
                string.IsNullOrWhiteSpace(quantityText) ||
                string.IsNullOrWhiteSpace(priceText) ||
                string.IsNullOrWhiteSpace(valueText))
            {
                MessageBox.Show("Please fill all fields before saving.");
                return;
            }

            try
            {
                int quantity = int.Parse(quantityText);
                double price = double.Parse(priceText);
                double value = double.Parse(valueText);

                byte[] imageBytes = ConvertImageToByteArray(SelectedImagePath); // Convert selected image to byte array

                using (var connection = new SQLiteConnection(@"Data Source=C:\Users\marlo\source\repos\Aszlit\MarlonStore\database\maindatabase.db"))
                {
                    connection.Open();
                    var command = new SQLiteCommand("INSERT INTO Inventory (ItemName, Quantity, Price, Value, Image) VALUES (@ItemName, @Quantity, @Price, @Value, @Image)", connection);
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Value", value);
                    command.Parameters.Add("@Image", System.Data.DbType.Binary).Value = imageBytes;
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Item added successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }


        private void SelectImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                PreviewImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                SelectedImagePath = openFileDialog.FileName; // Save the selected image path
            }
        }   

        private string SelectedImagePath { get; set; }







        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public Inventory InventoryControl { get; set; }

    }
}
