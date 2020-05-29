using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpTimeServer
{
    internal class TimeServer
    {
        private readonly int Delay;
        private readonly int Port;
        private bool Running;
        private readonly UdpClient Server;

        public TimeServer(int port, int delay)
        {
            Port = port;
            Delay = delay;
            Server = new UdpClient(Port);
        }

        public void Stop()
        {
            Running = false;
            lock (Server)
                Server.Dispose();
        }

        public async void StartAsync()
        {
            Running = true;
            while (Running)
            {
                try
                {
                    var request = await Server.ReceiveAsync();
                    Task.Run(() => SendAnswerAsync(Server, request));
                }
                catch
                {

                }
            }
        }

        private async Task SendAnswerAsync(UdpClient server, UdpReceiveResult request)
        {
            var response = Encoding.ASCII.GetBytes(GetDateWithDelay());
            await server.SendAsync(response, response.Length, request.RemoteEndPoint);
        }

        private string GetDateWithDelay()
        {
            var dateTime = DateTime.Now.Add(new TimeSpan(0, 0, 0, Delay));
            return
                $"Time is {dateTime.Hour}:{dateTime.Minute}:{dateTime.Second} Date is {dateTime.Day}.{dateTime.Month}.{dateTime.Year}";
        }
    }
}