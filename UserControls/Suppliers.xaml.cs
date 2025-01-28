using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Data.SQLite;

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
            SuppliersList.Clear();
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("SELECT supplier_name, contact, email, address, phone_number, website, status, date_added FROM Suppliers WHERE status != 'Inactive'", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DateTime dateAdded;
                        if (!DateTime.TryParse(reader["date_added"]?.ToString(), out dateAdded))
                        {
                            dateAdded = DateTime.MinValue; // Assign a default value if parsing fails
                        }

                        SuppliersList.Add(new Supplier
                        {
                            SupplierName = reader["supplier_name"]?.ToString() ?? string.Empty,
                            Contact = reader["contact"]?.ToString() ?? string.Empty,
                            Email = reader["email"]?.ToString() ?? string.Empty,
                            Address = reader["address"]?.ToString() ?? string.Empty,
                            PhoneNumber = reader["phone_number"]?.ToString() ?? string.Empty,
                            Website = reader["website"]?.ToString() ?? string.Empty,
                            Status = reader["status"]?.ToString() ?? string.Empty,
                            DateAdded = dateAdded
                        });
                    }
                }
            }
        }

        public void AddSupplier(Supplier newSupplier)
        {
            SuppliersList.Add(newSupplier);
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                var command = new SQLiteCommand("INSERT INTO Suppliers (supplier_name, contact, email, address, phone_number, website, status, date_added) VALUES (@SupplierName, @Contact, @Email, @Address, @PhoneNumber, @Website, @Status, @DateAdded)", connection);
                command.Parameters.AddWithValue("@SupplierName", newSupplier.SupplierName);
                command.Parameters.AddWithValue("@Contact", newSupplier.Contact);
                command.Parameters.AddWithValue("@Email", newSupplier.Email);
                command.Parameters.AddWithValue("@Address", newSupplier.Address);
                command.Parameters.AddWithValue("@PhoneNumber", newSupplier.PhoneNumber);
                command.Parameters.AddWithValue("@Website", newSupplier.Website);
                command.Parameters.AddWithValue("@Status", newSupplier.Status);
                command.Parameters.AddWithValue("@DateAdded", newSupplier.DateAdded);
                command.ExecuteNonQuery();
            }
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
            var addSupplierWindow = new AddSupplierWindow();
            if (addSupplierWindow.ShowDialog() == true)
            {
                AddSupplier(addSupplierWindow.NewSupplier);
                LoadSuppliers(); // Refresh the DataGrid
            }
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
        public string SupplierName { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string result = string.Empty;
                switch (columnName)
                {
                    case nameof(SupplierName):
                        if (string.IsNullOrWhiteSpace(SupplierName))
                            result = "Supplier Name is required.";
                        break;
                    case nameof(Email):
                        if (string.IsNullOrWhiteSpace(Email))
                            result = "Email is required.";
                        else if (!IsValidEmail(Email))
                            result = "Invalid email format.";
                        break;
                    case nameof(PhoneNumber):
                        if (string.IsNullOrWhiteSpace(PhoneNumber))
                            result = "Phone Number is required.";
                        break;
                        // Add more validation as needed
                }
                return result;
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
