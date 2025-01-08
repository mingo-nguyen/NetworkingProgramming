using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkingProgramming
{
    public class TcpOrderClient
    {
        private readonly string _serverIp;
        private readonly int _serverPort;

        public TcpOrderClient(string serverIp, int serverPort)
        {
            _serverIp = serverIp;
            _serverPort = serverPort;
        }

        public async Task<string> SendOrderAsync(string orderData)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    await client.ConnectAsync(_serverIp, _serverPort);
                    using (var networkStream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(orderData);
                        await networkStream.WriteAsync(data, 0, data.Length);

                        byte[] buffer = new byte[1024];
                        int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        Console.WriteLine($"Server response: {response}");
                        return response;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"SocketException: {ex.Message}");
                return $"SocketException: {ex.Message}";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return $"Exception: {ex.Message}";
            }
        }
    }
}
