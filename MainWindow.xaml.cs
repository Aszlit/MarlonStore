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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            // Only clear text if it's the placeholder text
            if (textBox.Text == "Enter text here...")
            {
                textBox.Clear(); // Clears the text
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {       
        
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
           
        }

        private void TextBox_TextChanged_2(object sender, TextChangedEventArgs e)
        {

        }
    }
}