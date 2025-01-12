using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Timers;



namespace Inventory
{
    public partial class LoginPage : Window
    {
        private int _loginAttempts = 0;
        private const int MaxLoginAttempts = 3;
        private const int CooldownPeriod = 30; // in seconds
        private System.Timers.Timer _cooldownTimer;

        public LoginPage()
        {
            InitializeComponent();
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize; // Disable resizing
            userinputlabel1.Visibility = Visibility.Hidden;
            userinputlabel2.Visibility = Visibility.Hidden;
        }

        // Close button event handler
        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Minimize button event handler
        private void MinimizeApp(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void forgotpage(object sender, MouseButtonEventArgs e)
        {
            RecoveryPage forgotWindow = new RecoveryPage
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            forgotWindow.Show();
            this.Close();
        }

        private void login(object sender, RoutedEventArgs e)
        {
            if (_loginAttempts >= MaxLoginAttempts)
            {
                MessageBox.Show($"Too many failed attempts. Please wait {CooldownPeriod} seconds before trying again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ResetFieldStyles();

            string username = input1.Text;
            string password = input2.Password;
            bool hasError = false;

            if (string.IsNullOrEmpty(username))
            {
                HighlightField(input1, userinputlabel1);
                input1.Focus();
                hasError = true;
            }

            if (string.IsNullOrEmpty(password))
            {
                HighlightField(input2, userinputlabel2);
                if (!hasError) input2.Focus();
                hasError = true;
            }

            if (hasError) return;

            if (IsLoginValid(username, password))
            {
                MainPage homepage = new MainPage();
                homepage.Show();
                this.Close();
            }
            else
            {
                _loginAttempts++;
                MessageBox.Show("Incorrect username or password. Please try again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);

                if (_loginAttempts >= MaxLoginAttempts)
                {
                    StartCooldown();
                }
                else
                {
                    HighlightField(input1, userinputlabel1);
                    HighlightField(input2, userinputlabel2);
                }
            }
        }

        private bool IsLoginValid(string username, string password)
        {
            string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database", "maindatabase.db");

            if (!File.Exists(databasePath))
            {
                MessageBox.Show($"Database file not found: {databasePath}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            string connectionString = $"Data Source={databasePath};Version=3;";

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
                catch (SQLiteException ex)
                {
                    MessageBox.Show($"SQLite Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        private void HighlightField(Control field, Label label)
        {
            field.BorderBrush = Brushes.Red;
            field.BorderThickness = new Thickness(2);
            label.Visibility = Visibility.Visible;
        }

        private void ResetFieldStyles()
        {
            input1.BorderBrush = Brushes.Gray;
            input1.BorderThickness = new Thickness(1);
            input2.BorderBrush = Brushes.Gray;
            input2.BorderThickness = new Thickness(1);
            userinputlabel1.Visibility = Visibility.Hidden;
            userinputlabel2.Visibility = Visibility.Hidden;
        }

        private void StartCooldown()
        {
            _cooldownTimer = new System.Timers.Timer(CooldownPeriod * 1000);
            _cooldownTimer.Elapsed += OnCooldownTimerElapsed;
            _cooldownTimer.AutoReset = false;
            _cooldownTimer.Start();
            MessageBox.Show($"Too many failed attempts. Please wait {CooldownPeriod} seconds before trying again.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void OnCooldownTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _cooldownTimer.Stop();
            _cooldownTimer.Dispose();
            _loginAttempts = 0;
            Dispatcher.Invoke(() =>
            {
                MessageBox.Show("You can now try logging in again.", "Cooldown Ended", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }
    }
}
