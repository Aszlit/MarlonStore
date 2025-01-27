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
        }

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

                        Items.Add(new Item
                        {
                            ItemName = reader["ItemName"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Price = Convert.ToDouble(reader["Price"]),
                            Value = Convert.ToDouble(reader["Value"]),
                            ProductImage = image,
                            Category = reader["Category"].ToString()
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

        public class Item
        {
            public string ItemName { get; set; }
            public int Quantity { get; set; }
            public double Price { get; set; }
            public double Value { get; set; }
            public BitmapImage ProductImage { get; set; }
            public string Category { get; set; }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
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

        private void SortOrderToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            SortOrderToggleButton.Content = "Descending";
            ApplySorting();
        }

        private void SortOrderToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            SortOrderToggleButton.Content = "Ascending";
            ApplySorting();
        }

        private void ApplySorting()
        {
            var sortDirection = SortOrderToggleButton.IsChecked == true ? ListSortDirection.Descending : ListSortDirection.Ascending;
            var sortCriteria = ((ComboBoxItem)SortCriteriaComboBox.SelectedItem).Content.ToString();

            InventoryDataGrid.Items.SortDescriptions.Clear();
            InventoryDataGrid.Items.SortDescriptions.Add(new SortDescription(sortCriteria, sortDirection));
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

    }
  
