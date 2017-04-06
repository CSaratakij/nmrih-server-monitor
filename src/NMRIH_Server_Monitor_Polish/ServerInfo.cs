using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace NMRIH
{
    public struct ServerInfo
    {
        public string Name;
        public string Host;
        public int Port;
        public string InitMap;
        public Process Process;
        public ProcessStartInfo ProcessStartInfo;

        public ServerInfo(string name, string host, int port, string command, string args, string map)
        {
            Name = name;
            Host = host;
            Port = port;
            InitMap = map;
            Process = null;
            ProcessStartInfo = new ProcessStartInfo(command, args);
        }
    }
}
