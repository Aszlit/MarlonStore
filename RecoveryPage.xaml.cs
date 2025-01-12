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
            // Logic to recover the password
        }
    }
}
