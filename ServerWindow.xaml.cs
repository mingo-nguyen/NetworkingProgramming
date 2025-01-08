using System;
using System.Windows;

namespace NetworkingProgramming
{
    public partial class ServerWindow : Window
    {
        private TcpOrderServer _tcpOrderServer;

        public ServerWindow()
        {
            InitializeComponent();
            _tcpOrderServer = new TcpOrderServer("127.0.0.1", 5000);
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

        private void OnOrderReceived(object sender, string orderData)
        {
            Dispatcher.Invoke(() => OrdersTextBox.AppendText(orderData + Environment.NewLine));
        }
    }
}
