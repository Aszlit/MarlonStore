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

namespace Inventory
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RecoveryPage : Window
    {
        public RecoveryPage()
        {
            InitializeComponent();
        }

        private void returnpage(object sender, RoutedEventArgs e)
        {
            // Logic to return to the login page
            LoginPage loginPage = new LoginPage();
            loginPage.Show();
            this.Close();
        }

        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void recover(object sender, RoutedEventArgs e)
        {
            string username = usernamebox.Text;
            string securityAnswer = securitybox.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(securityAnswer))
            {
                MessageBox.Show("Please enter both username and security answer.");
                return;
            }

            try
            {
                // Get the relative path to the database
                string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT Password FROM Users WHERE Username = @Username AND SecurityQuestion = @SecurityQuestion";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@SecurityQuestion", securityAnswer);
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string recoveredPassword = reader["Password"].ToString();
                                passrecoverlabel.Content = $"Recovered Password: {recoveredPassword}";
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or security answer.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
}
