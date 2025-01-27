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
using static Inventory.App;
using System.Globalization;
using System.ComponentModel;

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for Inventory.xaml
    /// </summary>
    public partial class Inventory : UserControl
    {
        public ObservableCollection<Item> Items { get; set; }

        public Inventory()
        {
            InitializeComponent();
            Items = new ObservableCollection<Item>();
            this.DataContext = this;
            LoadInventory();
            this.Loaded += Inventory_Loaded;
        }

        public void LoadInventory()
        {
            Items.Clear();

            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT * FROM Inventory WHERE Status != 'Inactive'", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        byte[] imageBytes = reader["Image"] as byte[];
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

                        DateTime dateAdded = reader["DateAdded"] != DBNull.Value ? Convert.ToDateTime(reader["DateAdded"]) : DateTime.MinValue;

                        Items.Add(new Item
                        {
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDouble(reader["Price"]),
                            Value = Convert.ToDouble(reader["Value"]),
                            ProductImage = image,
                            Category = reader["Category"].ToString(),
                            DateAdded = dateAdded,
                            Status = reader["Status"].ToString(),
                            Supplier = reader["Supplier"].ToString(), // Add Supplier
                            ExpirationDate = reader["ExpirationDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ExpirationDate"]) : null // Add ExpirationDate
                        });
                    }
                }
            }
        }

        private void addbutton(object sender, RoutedEventArgs e)
        {
            try
            {
                AddNewItemWindow addgoodswindow = new AddNewItemWindow();
                addgoodswindow.Owner = Window.GetWindow(this);
                addgoodswindow.ShowDialog();
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem is Item selectedItem)
            {
                var editWindow = new AddNewItemWindow
                {
                    Owner = Window.GetWindow(this),
                    EditingItem = selectedItem // Pass the selected item to the edit window
                };

                // Populate the edit window with the selected item's data
                editWindow.ItemNameTextBox.Text = selectedItem.ItemName;
                editWindow.QuantityTextBox.Value = selectedItem.Quantity;
                editWindow.PriceTextBox.Value = (int)selectedItem.Price;
                editWindow.SupplierComboBox.SelectedItem = selectedItem.Supplier;
                editWindow.ExpirationDatePicker.SelectedDate = selectedItem.ExpirationDate;
                editWindow.NoExpirationCheckBox.IsChecked = selectedItem.ExpirationDate == null;
                editWindow.ValueTextBox.Text = selectedItem.Value.ToString();
                editWindow.PreviewImage.Source = selectedItem.ProductImage;

                foreach (RadioButton radioButton in editWindow.CategoryPanel.Children)
                {
                    if (radioButton.Content.ToString() == selectedItem.Category)
                    {
                        radioButton.IsChecked = true;
                        break;
                    }
                }
                editWindow.ActiveRadioButton.IsChecked = selectedItem.Status == "Active";
                editWindow.InactiveRadioButton.IsChecked = selectedItem.Status == "Inactive";

                editWindow.SaveButton.Content = "Confirm";
                editWindow.addnewitemlabel.Text = "Edit Item";

                if (editWindow.ShowDialog() == true)
                {
                    // Update the selected item with the new data
                    selectedItem.ItemName = editWindow.ItemNameTextBox.Text;
                    selectedItem.Quantity = editWindow.QuantityTextBox.Value ?? 0;
                    selectedItem.Price = editWindow.PriceTextBox.Value ?? 0;
                    selectedItem.Supplier = editWindow.SupplierComboBox.SelectedItem.ToString();
                    selectedItem.ExpirationDate = editWindow.NoExpirationCheckBox.IsChecked == true ? null : editWindow.ExpirationDatePicker.SelectedDate;
                    selectedItem.Value = double.Parse(editWindow.ValueTextBox.Text);
                    selectedItem.Category = ((RadioButton)editWindow.CategoryPanel.Children.Cast<RadioButton>().FirstOrDefault(rb => rb.IsChecked == true)).Content.ToString();
                    selectedItem.Status = editWindow.ActiveRadioButton.IsChecked == true ? "Active" : "Inactive";
                    selectedItem.ProductImage = editWindow.PreviewImage.Source as BitmapImage;

                    UpdateItemInDatabase(selectedItem);
                    LoadInventory();
                }
            }
            else
            {
                MessageBox.Show("Please select an item to edit.", "No Item Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void refresh(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadInventory();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while refreshing: {ex.Message}");
            }
        }

        private void SortOrderButton_Click(object sender, RoutedEventArgs e)
        {
            ApplySorting();
        }

        private void SortCriteriaComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplySorting();
        }

        private void ApplySorting()
        {
            var selectedItem = SortCriteriaComboBox.SelectedItem;
            if (selectedItem is ComboBoxItem comboBoxItem && comboBoxItem.Content.ToString() == "Date Added")
            {
                SortByDateAdded();
                return;
            }

            var sortDirection = ListSortDirection.Ascending;
            if (SortOrderButton.Content.ToString() == "Sort Descending")
            {
                sortDirection = ListSortDirection.Descending;
                SortOrderButton.Content = "Sort Ascending";
            }
            else
            {
                SortOrderButton.Content = "Sort Descending";
            }

            var sortCriteria = ((ComboBoxItem)selectedItem).Content.ToString();

            InventoryDataGrid.Items.SortDescriptions.Clear();

            if (sortCriteria == "Stock Level Indicator")
            {
                sortCriteria = "Quantity";
            }

            InventoryDataGrid.Items.SortDescriptions.Add(new SortDescription(sortCriteria, sortDirection));
        }

        private void SortByDateAdded()
        {
            if (SortOrderButton == null)
            {
                MessageBox.Show("SortOrderButton is not initialized.");
                return;
            }

            if (InventoryDataGrid == null)
            {
                MessageBox.Show("InventoryDataGrid is not initialized.");
                return;
            }

            var sortDirection = ListSortDirection.Ascending;
            if (SortOrderButton.Content?.ToString() == "Sort Descending")
            {
                sortDirection = ListSortDirection.Descending;
                SortOrderButton.Content = "Sort Ascending";
            }
            else
            {
                SortOrderButton.Content = "Sort Descending";
            }

            ICollectionView dataView = CollectionViewSource.GetDefaultView(InventoryDataGrid.ItemsSource);
            if (dataView != null)
            {
                dataView.SortDescriptions.Clear();
                dataView.SortDescriptions.Add(new SortDescription("DateAdded", sortDirection));
                dataView.Refresh();
            }
        }

        private void Inventory_Loaded(object sender, RoutedEventArgs e)
        {
            SortByDateAdded();
        }

        private void removebutton_Click(object sender, RoutedEventArgs e)
        {
            if (InventoryDataGrid.SelectedItem is Item selectedItem)
            {
                selectedItem.Status = "Inactive";
                UpdateItemStatus(selectedItem);
                Items.Remove(selectedItem);
            }
        }

        private void UpdateItemStatus(Item item)
        {
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Inventory SET Status = 'Inactive' WHERE ItemName = @ItemName", connection);
                command.Parameters.AddWithValue("@ItemName", item.ItemName);
                command.ExecuteNonQuery();
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!InventoryDataGrid.IsMouseOver)
            {
                InventoryDataGrid.UnselectAll();
            }
        }
        private void UpdateItemInDatabase(Item item)
        {
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("UPDATE Inventory SET Quantity = @Quantity, Price = @Price, Value = @Value, Category = @Category, Status = @Status, Supplier = @Supplier, ExpirationDate = @ExpirationDate WHERE ItemName = @ItemName", connection);
                command.Parameters.AddWithValue("@ItemName", item.ItemName);
                command.Parameters.AddWithValue("@Quantity", item.Quantity);
                command.Parameters.AddWithValue("@Price", item.Price);
                command.Parameters.AddWithValue("@Value", item.Value);
                command.Parameters.AddWithValue("@Category", item.Category);
                command.Parameters.AddWithValue("@Status", item.Status);
                command.Parameters.AddWithValue("@Supplier", item.Supplier);
                command.Parameters.AddWithValue("@ExpirationDate", item.ExpirationDate.HasValue ? (object)item.ExpirationDate.Value : DBNull.Value);
                command.ExecuteNonQuery();
            }
        }
    }


    public class QuantityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int quantity)
            {
                if (quantity < 1)
                    return Brushes.Red;
                else if (quantity < 2)
                    return Brushes.Yellow;
                else
                    return Brushes.Green;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class PlaceholderToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is TextBlock textBlock && textBlock.Text == "Sort by")
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class Item
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double Value { get; set; }
        public BitmapImage ProductImage { get; set; }
        public string Category { get; set; }
        public DateTime DateAdded { get; set; }
        public string Status { get; set; } = "Active";
        public string Supplier { get; set; } // Add Supplier property
        public DateTime? ExpirationDate { get; set; } // Add ExpirationDate property
    }

    }
   
