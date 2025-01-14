using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : UserControl
    {
        private double _pricePerUnit;
        private int _availableQuantity;
        public event Action<string, int, double, BitmapImage> OrderAdded;

        public OrderDetails()
        {
            InitializeComponent();
        }

        public void SetProductDetails(BitmapImage image, string name, int quantity, double price)
        {
            ProductImage.Source = image;
            ProductName.Text = $"Name: {name}";
            ProductQuantity.Text = $"Quantity: {quantity}";
            ProductPrice.Text = $"Price: {price:C}";

            _pricePerUnit = price;
            _availableQuantity = quantity;
        }

        private void OrderQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(OrderQuantity.Text, out int orderQuantity))
            {
                double totalPrice = orderQuantity * _pricePerUnit;
                TotalPrice.Text = $"Total: {totalPrice:C}";
            }
            else
            {
                TotalPrice.Text = "Total: $0.00";
            }
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(OrderQuantity.Text, out int orderQuantity) && orderQuantity <= _availableQuantity)
            {
                // Raise event to notify PointSale
                OrderAdded?.Invoke(ProductName.Text.Replace("Name: ", ""), orderQuantity, _pricePerUnit, (BitmapImage)ProductImage.Source);

                MessageBox.Show("Order added successfully!");
            }
            else
            {
                MessageBox.Show("Invalid quantity or insufficient stock.");
            }
        }
    }
}
