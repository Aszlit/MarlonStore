using System;
using System.Collections.Generic;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
            this.ResizeMode = ResizeMode.NoResize;
            WindowStartupLocation = WindowStartupLocation;
    }

        private void logoutbutt(object sender, MouseButtonEventArgs e)
        {
            // Create the new window
            MainWindow logout = new();

            // Show the new window
            logout.Show();

            // Close the current window
            this.Close();
        }
    }
}
