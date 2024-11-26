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
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen; // Center the window
        }


        private void returnpage(object sender, MouseButtonEventArgs e)
        {
            // Create the new window
            MainWindow secondWindow = new();

            // Show the new window
            secondWindow.Show();

            // Close the current window
            this.Close();
        }


           

        private void recover(object sender, RoutedEventArgs e)
        {
            string username = usernamebox.Text;
            string securityAnswer = securitybox.Text;

            string recoveredPassword = RecoverPassword(username, securityAnswer);

            if (recoveredPassword != null)
            {
                passrecoverlabel.Content = $"Password: {recoveredPassword}";
            }
            else
            {
                passrecoverlabel.Content = "Invalid username or security answer!";
            }
        }

        private string RecoverPassword(string username, string securityAnswer)
        {
            string connectionString = "Data Source=C:\\Users\\marlo\\source\\repos\\Aszlit\\MarlonStore\\database\\maindatabase.db;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT Password 
                        FROM Users 
                        WHERE Username = @username AND SecurityQuestion = @securityAnswer";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@securityAnswer", securityAnswer);

                        object result = command.ExecuteScalar();
                        return result?.ToString(); // Return the password if found, null otherwise
                    }
                }
                catch
                {
                    MessageBox.Show("Error connecting to the database.");
                    return null;
                }
            }
        }
    }
}
