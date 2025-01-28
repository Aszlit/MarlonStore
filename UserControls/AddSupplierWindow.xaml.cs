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
            NewSupplier = new Supplier();
            this.DataContext = NewSupplier;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid(NewSupplier))
            {
                NewSupplier.DateAdded = DateAddedDatePicker.SelectedDate ?? DateTime.Now;
                DialogResult = true;
                Close();
            }
            else
            {
                MessageBox.Show("Please correct the errors before adding the supplier.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private bool IsValid(Supplier supplier)
        {
            var properties = typeof(Supplier).GetProperties();
            foreach (var property in properties)
            {
                if (supplier[property.Name] != null)
                {
                    return false;
                }
            }
            return true;
        }

        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
