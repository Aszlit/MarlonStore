using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        public MainPage()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation;

            // Initialize the database before any operations (without table creation)
            InitializeDatabase();
        }

        // Logout Button
        private void logoutbutt(object sender, MouseButtonEventArgs e)
        {
            // Create the new window (MainWindow)
            LoginPage logout = new LoginPage();

            // Show the new window
            logout.Show();

            // Close the current window
            this.Close();
        }

        // Navigate to Inventory
        private void inventory(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Visible;
            PurchasesGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Home
        private void home(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Visible;
            InventoryGrid.Visibility = Visibility.Collapsed;
            PurchasesGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Purchases
        private void purchases(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            PurchasesGrid.Visibility = Visibility.Visible;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Sales
        private void sales(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            PurchasesGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Visible;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        private void suppliers(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            PurchasesGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Visible;
            AboutGrid.Visibility = Visibility.Collapsed;
        }


        private void about(object sender, MouseButtonEventArgs e)
        {

            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            PurchasesGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Visible;
        }




        // Method to initialize the database connection (without creating the table)
        public static void InitializeDatabase()
        {
            // Path to SQLite database file
            string connectionString = "Data Source=C:\\Users\\marlo\\source\\repos\\Aszlit\\MarlonStore\\database\\maindatabase.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Simply open the connection to ensure it's working, no need to create the table.
                    connection.Close();
                }
                catch (Exception ex)
                {
                    // Handle database connection errors
                    MessageBox.Show("Error initializing database: " + ex.Message);
                }
            }
        }
    }
}