using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using static Inventory.App;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class MainPage : Window
    {
        private Label _selectedLabel;

        public MainPage()
        {
            try
            {
                UserContext.mainPage = this;
                InitializeComponent();
                this.WindowState = WindowState.Maximized;
                this.ResizeMode = ResizeMode.NoResize;
                WindowStartupLocation = WindowStartupLocation;

                // Initialize the database before any operations (without table creation)
                InitializeDatabase();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while initializing the MainPage: {ex.Message}", ex);
                Application.Current.Shutdown();
            }
        }

        // Logout Button
        private void logoutbutt(object sender, MouseButtonEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while logging out: {ex.Message}", ex);
            }
        }

        // Navigate to Inventory
        private void inventory(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                inventoryLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = inventoryLabel;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Visible;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to Inventory: {ex.Message}", ex);
            }
        }

        // Navigate to Home
        private void home(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                homeLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = homeLabel;

                HomepageGrid.Visibility = Visibility.Visible;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to Home: {ex.Message}", ex);
            }
        }

        // Navigate to Purchases
        private void Records(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                recordsLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = recordsLabel;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Visible;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to Records: {ex.Message}", ex);
            }
        }

        // Navigate to Sales
        private void sales(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                salesLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = salesLabel;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Visible;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to Sales: {ex.Message}", ex);
            }
        }

        private void suppliers(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                suppliersLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = suppliersLabel;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Visible;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to Suppliers: {ex.Message}", ex);
            }
        }

        private void about(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                aboutLabel.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = aboutLabel;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Visible;
                PointSaleGrid.Visibility = Visibility.Collapsed;

                // Show the rectangles
                rectangle1.Visibility = Visibility.Visible;
                rectangle2.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to About: {ex.Message}", ex);
            }
        }

        private void pos(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResetLabelBackgrounds();
                aboutLabel_Copy.Background = new SolidColorBrush(Color.FromRgb(45, 45, 44)); // Highlight color
                _selectedLabel = aboutLabel_Copy;

                HomepageGrid.Visibility = Visibility.Collapsed;
                InventoryGrid.Visibility = Visibility.Collapsed;
                RecordsGrid.Visibility = Visibility.Collapsed;
                SalesGrid.Visibility = Visibility.Collapsed;
                SuppliersGrid.Visibility = Visibility.Collapsed;
                AboutGrid.Visibility = Visibility.Collapsed;
                PointSaleGrid.Visibility = Visibility.Visible;

                // Hide the rectangles
                rectangle1.Visibility = Visibility.Collapsed;
                rectangle2.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while navigating to POS: {ex.Message}", ex);
            }
        }

        // MouseEnter Event Handler
        private void Label_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                Label label = sender as Label;

                if (label != null && label != _selectedLabel)
                {
                    // Change background color to a lighter shade when hovered
                    label.Background = new SolidColorBrush(Color.FromRgb(91, 91, 91)); // Lighter shade
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while handling MouseEnter event: {ex.Message}", ex);
            }
        }

        // MouseLeave Event Handler
        private void Label_MouseLeave(object sender, MouseEventArgs e)
        {
            try
            {
                Label label = sender as Label;

                if (label != null && label != _selectedLabel)
                {
                    // Revert back to the original background color
                    label.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while handling MouseLeave event: {ex.Message}", ex);
            }
        }

        // Method to reset all label backgrounds to the default color
        private void ResetLabelBackgrounds()
        {
            try
            {
                homeLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                inventoryLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                recordsLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                salesLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                suppliersLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                aboutLabel.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
                aboutLabel_Copy.Background = new SolidColorBrush(Color.FromRgb(70, 70, 70)); // Original dark shade
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"An error occurred while resetting label backgrounds: {ex.Message}", ex);
            }
        }

        // Method to initialize the database connection (without creating the table)
        public static void InitializeDatabase()
        {
            try
            {
                // Path to SQLite database file
                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    // Simply open the connection to ensure it's working, no need to create the table.
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                // Handle database connection errors
                ShowErrorMessage($"Error initializing database: {ex.Message}", ex);
            }
        }

        // Method to show error messages with a "Copy to Clipboard" button
        private static void ShowErrorMessage(string message, Exception ex)
        {
            var errorMessage = $"{message}\n\n{ex}";
            var window = new Window
            {
                Title = "Error",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var grid = new Grid();
            var textBox = new TextBox
            {
                Text = errorMessage,
                IsReadOnly = true,
                TextWrapping = TextWrapping.Wrap,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(10)
            };
            var button = new Button
            {
                Content = "Copy to Clipboard",
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(10)
            };
            button.Click += (s, e) => Clipboard.SetText(errorMessage);

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Children.Add(textBox);
            grid.Children.Add(button);
            Grid.SetRow(textBox, 0);
            Grid.SetRow(button, 1);

            window.Content = grid;
            window.ShowDialog();
        }
    }
}