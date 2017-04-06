using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NMRIH
{
    public class Client
    {
        public bool Connecting { get; private set; }
        public bool Connected { get; private set; } // If tcp is already checking connection
        public bool Online { get; private set; }

        public Client()
        {
            Connecting = false;
            Connected = false;
            Online = false;
        }

        public async Task<bool> ConnectAsync(Server server)
        {
            var tcp = new TcpClient();
            tcp.SendTimeout = 3000;
            tcp.ReceiveTimeout = 3000;
            tcp.NoDelay = true;
            try
            {
                Connected = false;
                Connecting = true;
                Console.WriteLine("Connecting to server : Port : {0}", server.Info.Port);
                await tcp.ConnectAsync(server.Info.Host, server.Info.Port);
                Console.WriteLine("Connected to server : Port : {0}", server.Info.Port);
            }
            catch { }
            Connecting = false;
            Connected = true;
            Online = tcp.Connected;
            tcp.Close();
            return Online;
        }
    }
}
