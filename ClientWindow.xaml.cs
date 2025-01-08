using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkingProgramming
{
    public partial class MainWindow : Window
    {
        private readonly TcpOrderClient _tcpOrderClient;
        private readonly UdpOrderBroadcaster _udpOrderBroadcaster;

        public MainWindow()
        {
            InitializeComponent();
            _tcpOrderClient = new TcpOrderClient("127.0.0.1", 5000);
            _udpOrderBroadcaster = new UdpOrderBroadcaster("255.255.255.255", 6000);
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string orderData = $"OrderID: {OrderIdTextBox.Text}, FoodList: {FoodListTextBox.Text}, DeliveryAddress: {DeliveryAddressTextBox.Text}";
                string response = await _tcpOrderClient.SendOrderAsync(orderData);
                await _udpOrderBroadcaster.BroadcastNewOrderAsync("New Order Placed: " + orderData);
                ResponseTextBox.Text = response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending order: {ex.Message}");
                ResponseTextBox.Text = $"Error: {ex.Message}";
            }
        }

        private void RemovePlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = Brushes.Black;
            }
        }

        private void AddPlaceholderText(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Foreground = Brushes.Gray;
                textBox.Text = textBox.Name switch
                {
                    "OrderIdTextBox" => "Order ID",
                    "FoodListTextBox" => "Food List",
                    "DeliveryAddressTextBox" => "Delivery Address",
                    _ => textBox.Text
                };
            }
        }
    }
}
