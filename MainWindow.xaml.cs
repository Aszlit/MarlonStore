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
    }
}