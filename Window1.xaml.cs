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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }


        private void returnpage(object sender, MouseButtonEventArgs e)
        {
            // Create the new window
            MainWindow secondWindow = new MainWindow();
            secondWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen; // Center the window

            // Show the new window
            secondWindow.Show();

            // Close the current window
            this.Close();
        }

    }
}
