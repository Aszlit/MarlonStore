using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for Suppliers.xaml
    /// </summary>
    public partial class Suppliers : UserControl
    {
        public ObservableCollection<Supplier> SuppliersList { get; set; }

        public Suppliers()
        {
            InitializeComponent();
            SuppliersList = new ObservableCollection<Supplier>();
            this.DataContext = this;
            LoadSuppliers();
        }

        public void LoadSuppliers()
        {
            // Load suppliers from database or any data source
            // For demonstration, adding dummy data
            SuppliersList.Add(new Supplier { SupplierName = "Supplier 1", Contact = "1234567890", Email = "supplier1@example.com", Address = "Address 1" });
            SuppliersList.Add(new Supplier { SupplierName = "Supplier 2", Contact = "0987654321", Email = "supplier2@example.com", Address = "Address 2" });
            SuppliersList.Add(new Supplier { SupplierName = "Supplier 3", Contact = "1112223333", Email = "supplier3@example.com", Address = "Address 3" });
            SuppliersList.Add(new Supplier { SupplierName = "Supplier 4", Contact = "4445556666", Email = "supplier4@example.com", Address = "Address 4" });
            SuppliersList.Add(new Supplier { SupplierName = "Supplier 5", Contact = "7778889999", Email = "supplier5@example.com", Address = "Address 5" });
        }

        public void AddSupplier(Supplier newSupplier)
        {
            SuppliersList.Add(newSupplier);
        }

        public void EditSupplier(int index, Supplier editedSupplier)
        {
            if (index >= 0 && index < SuppliersList.Count)
            {
                SuppliersList[index] = editedSupplier;
            }
        }

        public void DeleteSupplier(int index)
        {
            if (index >= 0 && index < SuppliersList.Count)
            {
                SuppliersList.RemoveAt(index);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Add new supplier logic
            MessageBox.Show("Add button clicked");
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            // Edit selected supplier logic
            MessageBox.Show("Edit button clicked");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Delete selected supplier logic
            MessageBox.Show("Delete button clicked");
        }
    }

    public class Supplier
    {
        public string SupplierName { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
