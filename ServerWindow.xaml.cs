using System;
using System.Windows;

namespace NetworkingProgramming
{
    public partial class ServerWindow : Window
    {
        private TcpOrderServer _tcpOrderServer;
        private UdpOrderBroadcaster _udpOrderBroadcaster;

        public ServerWindow()
        {
            InitializeComponent();
            _udpOrderBroadcaster = new UdpOrderBroadcaster("255.255.255.255", 6000);
            _tcpOrderServer = new TcpOrderServer("127.0.0.1", 5000, _udpOrderBroadcaster);
        }

        private async void StartServerButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _tcpOrderServer.OrderReceived += OnOrderReceived;
                await _tcpOrderServer.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting TCP server: {ex.Message}");
            }
        }

        private async void SendNotificationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string notificationMessage = "New Order Placed";
                await _udpOrderBroadcaster.BroadcastNewOrderAsync(notificationMessage);
                OrdersTextBox.AppendText($"Notification sent: {notificationMessage}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
                OrdersTextBox.AppendText($"Error: {ex.Message}" + Environment.NewLine);
            }
        }

        private void OnOrderReceived(object sender, string orderData)
        {
            Dispatcher.Invoke(() => OrdersTextBox.AppendText(orderData + Environment.NewLine));
        }
    }
}
