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

namespace Inventory.UserControls
{
    /// <summary>
    /// Interaction logic for AddSupplierWindow.xaml
    /// </summary>
    public partial class AddSupplierWindow : Window
    {
        public Supplier NewSupplier { get; private set; }

        public AddSupplierWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            NewSupplier = new Supplier
            {
                SupplierName = SupplierNameTextBox.Text,
                Contact = ContactTextBox.Text,
                Email = EmailTextBox.Text,
                Address = AddressTextBox.Text
            };

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
