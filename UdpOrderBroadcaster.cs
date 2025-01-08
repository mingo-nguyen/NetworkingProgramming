using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingProgramming
{
    public class UdpOrderBroadcaster
    {
        private UdpClient _udpClient;
        private IPEndPoint _endPoint;

        public UdpOrderBroadcaster(string broadcastIp, int port)
        {
            _udpClient = new UdpClient();
            _endPoint = new IPEndPoint(IPAddress.Parse(broadcastIp), port);
        }

        public async Task BroadcastNewOrderAsync(string orderData)
        {
            byte[] data = Encoding.UTF8.GetBytes(orderData);
            await _udpClient.SendAsync(data, data.Length, _endPoint);
            Console.WriteLine("Broadcasted new order notification.");
        }
    }
}
