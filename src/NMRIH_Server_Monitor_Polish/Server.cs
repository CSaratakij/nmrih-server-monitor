using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NMRIH
{
    public class Server
    {
        ServerInfo info;

        public ServerInfo Info { get { return info; } }


        public Server(ServerInfo info)
        {
            this.info = info;
        }

        public void Start()
        {
            info.Process = Process.Start(info.ProcessStartInfo);
        }

        public void Stop()
        {
            if (info.Process != null && !info.Process.HasExited)
                info.Process.Kill();
        }

        public void Restart()
        {
            Stop();
            Start();
        }
    }
}
