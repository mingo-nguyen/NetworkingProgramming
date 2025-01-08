using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetworkingProgramming
{
    public partial class MainWindow : Window
    {
        private readonly TcpOrderClient _tcpOrderClient;
        private static int _orderCounter = 1;
        private UdpClient _udpClient;

        public MainWindow()
        {
            InitializeComponent();
            _tcpOrderClient = new TcpOrderClient("127.0.0.1", 5000);
            StartUdpListener();
        }

        private async void SendOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string orderId = _orderCounter.ToString("D5");
                string orderData = $"OrderID: {orderId}, FoodList: {FoodListTextBox.Text}, DeliveryAddress: {DeliveryAddressTextBox.Text}";
                string response = await _tcpOrderClient.SendOrderAsync(orderData);
                ResponseTextBox.AppendText($"OrderID: {orderId} - {response}" + Environment.NewLine);
                _orderCounter++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending order: {ex.Message}");
                ResponseTextBox.AppendText($"Error: {ex.Message}" + Environment.NewLine);
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

        private void StartUdpListener()
        {
            _udpClient = new UdpClient(6000);
            Task.Run(async () =>
            {
                while (true)
                {
                    var result = await _udpClient.ReceiveAsync();
                    string message = Encoding.UTF8.GetString(result.Buffer);
                    Dispatcher.Invoke(() => NotificationTextBox.AppendText(message + Environment.NewLine));
                }
            });
        }
    }
}
