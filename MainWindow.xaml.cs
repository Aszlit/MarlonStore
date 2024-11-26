using System.Data.SQLite;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void forgotpage(object sender, MouseButtonEventArgs e)
        {
            // Create the new window
            Window1 forgotWindow = new Window1();
            forgotWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen; // Center the window

            // Show the new window
            forgotWindow.Show();

            // Close the current window
            this.Close();
        }



        private void login(object sender, RoutedEventArgs e)
        {
            string username = input1.Text;  // Get username from input1
            string password = input2.Password;  // Get password from input2

            if (IsLoginValid(username, password))
            {
                MessageBox.Show("Login successful!");
                // Redirect to homepage
                Window2 homepage = new Window2(); // Replace with your actual homepage
                homepage.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }

        private bool IsLoginValid(string username, string password)
        {
            string connectionString = "Data Source=C:\\Users\\marlo\\source\\repos\\Aszlit\\MarlonStore\\database\\maindatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM Users WHERE Username = @username AND Password = @password";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@password", password);

                        int result = Convert.ToInt32(command.ExecuteScalar());
                        return result == 1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}");
                    return false;
                }
            }
        }
    }
}