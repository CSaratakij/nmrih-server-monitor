using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMRIH
{
    class ServerManager
    {
        public bool Started { get; private set; }
        // should have reload queue var

        public ServerManager(Setting setting, List<Server> servers, List<Client> clients)
        {
            Started = false;
        }

        // while loop like -> Game loop (except, just like sqeuedue)
        public void ManageServer()
        {

        }

        // Start with delay on each process spawn
        public void StartAllServer()
        {
            // init timer and bind to Manage Server <- 1.3 min things (u know :)
            Started = true;
        }

        public void CheckServerConnection()
        {

        }
    }
}
