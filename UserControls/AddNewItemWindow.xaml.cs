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
using System.IO;
using System.Text.RegularExpressions;
using System.Drawing;

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
            LoadSuppliers();
        }

        // Close the window
        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        // Save Button Click
        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var itemName = ItemNameTextBox.Text;
            var quantityText = QuantityTextBox.Text;
            var priceText = PriceTextBox.Text;
            var valueText = ValueTextBox.Text;
            var category = GetSelectedCategory();
            var dateAdded = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var status = GetSelectedStatus();
            var supplier = SupplierComboBox.Text; // Ensure the supplier is correctly retrieved
            var expirationDate = NoExpirationCheckBox.IsChecked == true ? (DateTime?)null : ExpirationDatePicker.SelectedDate;

            // Validate inputs
            if (string.IsNullOrWhiteSpace(itemName) ||
                string.IsNullOrWhiteSpace(quantityText) ||
                string.IsNullOrWhiteSpace(priceText) ||
                string.IsNullOrWhiteSpace(valueText) ||
                string.IsNullOrWhiteSpace(category) ||
                string.IsNullOrWhiteSpace(supplier)) // Add supplier validation
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

                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("INSERT INTO Inventory (ItemName, Quantity, Price, Value, Category, Image, DateAdded, Status, Supplier, ExpirationDate) VALUES (@ItemName, @Quantity, @Price, @Value, @Category, @Image, @DateAdded, @Status, @Supplier, @ExpirationDate)", connection);
                    command.Parameters.AddWithValue("@ItemName", itemName);
                    command.Parameters.AddWithValue("@Quantity", quantity);
                    command.Parameters.AddWithValue("@Price", price);
                    command.Parameters.AddWithValue("@Value", value);
                    command.Parameters.AddWithValue("@Category", category);
                    command.Parameters.Add("@Image", System.Data.DbType.Binary).Value = imageBytes;
                    command.Parameters.AddWithValue("@DateAdded", dateAdded);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@Supplier", supplier); // Ensure the supplier parameter is added
                    command.Parameters.AddWithValue("@ExpirationDate", (object)expirationDate ?? DBNull.Value); // Add expiration date parameter
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



        private void NoExpirationCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ExpirationDatePicker.IsEnabled = false;
            ExpirationDatePicker.SelectedDate = null;
        }

        private void NoExpirationCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ExpirationDatePicker.IsEnabled = true;
        }




        // Get selected category
        private string GetSelectedCategory()
        {
            foreach (var child in CategoryPanel.Children)
            {
                if (child is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    return radioButton.Content.ToString();
                }
            }
            return null;
        }


        // Get selected status
        private string GetSelectedStatus()
        {
            if (ActiveRadioButton.IsChecked == true)
            {
                return "Active";
            }
            else if (InactiveRadioButton.IsChecked == true)
            {
                return "Inactive";
            }
            return null;
        }



        // Convert image to byte array
        private byte[] ConvertImageToByteArray(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return null;

            return File.ReadAllBytes(imagePath); // Read the original image file as byte array
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

        private void QuantityTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private void QuantityTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Left || e.Key == Key.Right)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = !IsTextAllowed(e.Key.ToString());
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+"); // Regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void QuantityOrPrice_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (QuantityTextBox.Value.HasValue && PriceTextBox.Value.HasValue)
            {
                int quantity = QuantityTextBox.Value.Value;
                int price = PriceTextBox.Value.Value;
                int value = quantity * price;
                ValueTextBox.Text = value.ToString();
            }
            else
            {
                ValueTextBox.Text = string.Empty;
            }
        }

        private void InsertImageButton_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePath = openFileDialog.FileName;
                var resizedImage = ResizeImage(SelectedImagePath, 100, 100); // Resize to fit the preview box
                image_box.Source = BitmapToImageSource(resizedImage);
            }
        }

        private Bitmap ResizeImage(string imagePath, int width, int height)
        {
            using (var originalImage = new Bitmap(imagePath))
            {
                var resizedImage = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(originalImage, 0, 0, width, height);
                }
                return resizedImage;
            }
        }

        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }
    

    private void LoadSuppliers()
        {
            try
            {
                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    var command = new SQLiteCommand("SELECT supplier_name FROM Suppliers", connection);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SupplierComboBox.Items.Add(reader["supplier_name"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while loading suppliers: {ex.Message}");
            }
        }

        }
    }

