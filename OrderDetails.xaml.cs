﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory
{
    /// <summary>
    /// Interaction logic for OrderDetails.xaml
    /// </summary>
    public partial class OrderDetails : UserControl
    {
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
        }
    }
}
