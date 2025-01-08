using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingProgramming
{
    public class TcpOrderServer
    {
        private TcpListener _listener;
        private UdpOrderBroadcaster _udpOrderBroadcaster;

        public event EventHandler<string> OrderReceived;

        public TcpOrderServer(string ipAddress, int port, UdpOrderBroadcaster udpOrderBroadcaster)
        {
            _listener = new TcpListener(IPAddress.Parse(ipAddress), port);
            _udpOrderBroadcaster = udpOrderBroadcaster;
        }

        public async Task StartAsync()
        {
            try
            {
                _listener.Start();
                Console.WriteLine("Server started and listening...");

                while (true)
                {
                    var client = await _listener.AcceptTcpClientAsync();
                    _ = HandleClientAsync(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in server: {ex.Message}");
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (var networkStream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                string orderData = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                Console.WriteLine($"Received order: {orderData}");
                OrderReceived?.Invoke(this, orderData);

                // Send acknowledgment
                byte[] response = Encoding.UTF8.GetBytes("Order Received");
                await networkStream.WriteAsync(response, 0, response.Length);
            }
        }
    }
}

