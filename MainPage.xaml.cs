using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

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
            // Show a confirmation dialog
            MessageBoxResult result = MessageBox.Show("Are you sure you want to log out?", "Logout Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            // Check if the user clicked "Yes"
            if (result == MessageBoxResult.Yes)
            {
                // Create the new window (LoginPage)
                LoginPage logout = new LoginPage();

                // Show the new window
                logout.Show();

                // Close the current window
                this.Close();
            }
        }

        // Navigate to Inventory
        private void inventory(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Visible;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
            PointSaleGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Home
        private void home(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Visible;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Purchases
        private void Records(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Visible;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
            PointSaleGrid.Visibility = Visibility.Collapsed;
        }

        // Navigate to Sales
        private void sales(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Visible;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
        }

        private void suppliers(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Visible;
            AboutGrid.Visibility = Visibility.Collapsed;
            PointSaleGrid.Visibility = Visibility.Collapsed;
        }


        private void about(object sender, MouseButtonEventArgs e)
        {

            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Visible;
            PointSaleGrid.Visibility = Visibility.Collapsed;
        }

        private void pos(object sender, MouseButtonEventArgs e)
        {
            HomepageGrid.Visibility = Visibility.Collapsed;
            InventoryGrid.Visibility = Visibility.Collapsed;
            RecordsGrid.Visibility = Visibility.Collapsed;
            SalesGrid.Visibility = Visibility.Collapsed;
            SuppliersGrid.Visibility = Visibility.Collapsed;
            AboutGrid.Visibility = Visibility.Collapsed;
            PointSaleGrid.Visibility = Visibility.Visible;
        }


        // MouseEnter Event Handler
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;

            if (label != null)
            {
                // Change background color to a lighter shade when hovered
                label.Background = new SolidColorBrush(Color.FromRgb(91, 91, 91)); // Lighter shade
            }
        }

        // MouseLeave Event Handler
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            Label label = sender as Label;

            if (label != null)
            {
                // Revert back to the original background color
                label.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
            }
        }

    



        // Method to initialize the database connection (without creating the table)
        public static void InitializeDatabase()
        {
            // Path to SQLite database file
            string connectionString = "Data Source=C:\\Users\\marlo\\source\\repos\\MarlonStore\\database\\maindatabase.db;Version=3;";

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