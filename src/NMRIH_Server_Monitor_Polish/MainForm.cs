using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NMRIH
{
    public partial class MainForm : Form
    {
        const string HOST = "nurseryms.ddns.net";
        const int INIT_PORT = 28010;
        const int MAX_SERVER = 10;

        const int START_SERVER_DELAY = 800;
        const int UPDATE_VIEW_DELAY = 5000;
        const int CHECK_CONNECTION_DELAY = 90000;
        const int FINAL_TEST_CONNECTION_DELAY = 30000;

        const string COMMAND = "srcds.exe";
        const string COMMON_COMMAND_ARGS = "-game nmrih -insecure -console -nogui";


        Random dice;

        string[] server_names =
        {
           "Ranking Server [Nightmare, Realism, FriendlyFire, Dead]",
           "Thailand Server [Normal & DeadTalk/Alltalk]",
           "Singapore Server [Normal & DeadTalk/Alltalk]",
           "Phillippines Server [Normal & DeadTalk/Alltalk]",
           "Indonesia Server [Normal & DeadTalk/Alltalk]",
           "Asia Server [Normal & DeadTalk/Alltalk]",
           "Asia Server [Normal & DeadTalk/Alltalk]",
           "Asia Server [Normal & DeadTalk/Alltalk]",
           "Asia Server [Normal & DeadTalk/Alltalk]",
           "Dodgeball Server [Normal & DeadTalk/Alltalk]",
        };

        List<Server> servers;
        List<Client> clients;

        List<int> restartQueues;         // Queue to test connection (2nd attempt)
        List<int> confirmRestartQueues;  //Actual queue to restart server

        List<string> loadedMap;

        AutoResetEvent autoResetEvent;

        System.Threading.Timer updateViewTimer;
        System.Threading.Timer checkStatusTimer;


        public MainForm()
        {
            InitializeComponent();
        }

        private void UpdateViewTrigger(object stateInfo)
        {
            // If ServerManager start all server -> create new ui thread
            BeginInvoke(new MethodInvoker(() => UpdateView()));
        }

        private void UpdateView()
        {
            for (int i = 0; i < servers.Count; i++)    // Max server please
            {
                var textOnlineStatus = "Unknown";
                var textProcessStatus = "Unknown";
                var textUptime = "Unknown";

                if (clients[i].Connecting)
                    textOnlineStatus = "Connecting...";
                else
                {
                    if (clients[i].Online)
                    {
                        textOnlineStatus = "Online";
                        if (servers[i].Info.Process != null && !servers[i].Info.Process.HasExited)
                        {
                            var uptime = DateTime.Now - servers[i].Info.Process.StartTime;
                            textUptime = String.Format("{0} : {1} : {2}",
                                uptime.Hours,
                                uptime.Minutes,
                                uptime.Seconds);
                        }
                        else
                            textUptime = "Unknown";
                    }
                    else
                    {
                        textOnlineStatus = "Offline";
                        textUptime = "Unknown";
                    }
                }

                if (servers[i].Info.Process == null || servers[i].Info.Process.HasExited)
                {
                    textProcessStatus = "Terminate";
                    textUptime = "Unknown";
                }
                else
                {
                    if (servers[i].Info.Process.Responding)
                        textProcessStatus = "Running";
                    else
                        textProcessStatus = "Not Responding";
                }

                lstServerStat.Items[i].SubItems[3].Text = textOnlineStatus;
                lstServerStat.Items[i].SubItems[4].Text = textProcessStatus;
                lstServerStat.Items[i].SubItems[5].Text = textUptime;
            }
        }

        // Should check every 1.30 min
        // And add 2nd check (with delay) -> If server can't connect.
        // If 2nd attempt cannot connect to server -> Restart Server
        // And this should be in 'ServerManager'
        private async void TestConnection(object stateInfo)
        {
            var testConnectionTask = new Task[MAX_SERVER];

            Console.WriteLine("Check Connection now..");
            for (int i = 0; i < servers.Count; i++)
            {
                testConnectionTask[i] = clients[i].ConnectAsync(servers[i]);
            }

            await Task.WhenAll(testConnectionTask);

            // Added offline server to restart queue
            for (int i = 0; i < servers.Count; i++)
            {
                if (clients[i].Connected)
                {
                    if (!clients[i].Online)
                    {
                        Console.WriteLine("Added restartQueue : Server -> ({0})", i + 1);
                        restartQueues.Add(i);
                    }
                }
            }

            // Check offline server if is online? (after 30 sec delay)
            if (restartQueues.Count > 0)
            {
                var finalTestConnectionTask = new Task[restartQueues.Count];

                for (int i = 0; i < restartQueues.Count; i++)
                {
                    Console.WriteLine("Testing final connection : Server -> ({0})", restartQueues[i] + 1);
                    finalTestConnectionTask[i] = Task.Delay(FINAL_TEST_CONNECTION_DELAY).ContinueWith(async t => await clients[restartQueues[i]].ConnectAsync(servers[restartQueues[i]]));
                }

                Task.WaitAll(finalTestConnectionTask);

                for (int i = 0; i < restartQueues.Count; i++)
                {
                    if (clients[restartQueues[i]].Connected)
                    {
                        if (!clients[restartQueues[i]].Online)
                        {
                            confirmRestartQueues.Add(restartQueues[i]);
                            Console.WriteLine("Added Confirm Restart : Server -> ({0})", restartQueues[i] + 1);
                        }
                    }
                }

                restartQueues.Clear();
                Console.WriteLine("Clear all restart queue...");

                //After second test -> Restart server here
                if (confirmRestartQueues.Count > 0)
                {
                    Console.WriteLine("Begin : Actual Restart Server");
                    var delay = START_SERVER_DELAY;

                    for (int i = 0; i < confirmRestartQueues.Count; i++)
                    {
                        // Random map here
                        var indexInitMap = 0;
                        var initMap = "";

                        if ((confirmRestartQueues[i] + 1) == 10)
                        {
                            initMap = "nmo_dodgeball_v5";
                        }
                        else
                        {
                            if ((confirmRestartQueues[i] + 1) == 9)
                                indexInitMap = dice.Next(loadedMap.Count);
                            else
                                indexInitMap = dice.Next(10);

                            initMap = loadedMap[indexInitMap];
                        }

                        Console.WriteLine("Result gen map Server ({0}) : {1}", (confirmRestartQueues[i] + 1).ToString(), initMap);

                        //"srcds.exe -game nmrih -insecure -console -nogui -port 28010 +exec server1.cfg +map nmo_fema";
                        var commandArgs = COMMON_COMMAND_ARGS;
                        commandArgs += " -port " + servers[confirmRestartQueues[i]].Info.Port;
                        commandArgs += " +exec " + "server" + (confirmRestartQueues[i] + 1) + ".cfg";
                        commandArgs += " +map " + initMap;           // <- Change to random map later

                        servers[confirmRestartQueues[i]].Info.ProcessStartInfo.Arguments = commandArgs;
                        Console.WriteLine("Result Command Server ({0}) : {1} {2}", (confirmRestartQueues[i] + 1).ToString(), COMMAND, commandArgs);

                        // Restart Server with delay here 
                        Console.WriteLine("Restart Server : {0}", confirmRestartQueues[i] + 1);
                        await Task.Delay(delay).ContinueWith(t => servers[confirmRestartQueues[i]].Restart());
                        delay += START_SERVER_DELAY;
                    }

                    confirmRestartQueues.Clear();
                    Console.WriteLine("Clear all confirm restart queue...");
                }
            }
        }

        // Test 3 screnario
        // 1st all server is online
        // 2nd : only 1 server is down
        // 3rd : multiple servers is down
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadConfig();
            Init();
            InitView();

            autoResetEvent = new AutoResetEvent(false);

            updateViewTimer = new System.Threading.Timer(UpdateViewTrigger, autoResetEvent, 0, UPDATE_VIEW_DELAY);
            checkStatusTimer = new System.Threading.Timer(TestConnection, autoResetEvent, 3000, CHECK_CONNECTION_DELAY);
        }

        public void Init()
        {
            servers = new List<Server>();
            clients = new List<Client>();

            restartQueues = new List<int>();
            confirmRestartQueues = new List<int>();

            // Load map from text file here
            var mapTextFileName = "maps.txt";
            loadedMap = new List<String>();

            if (File.Exists(mapTextFileName))
            {
                var readedText = "";
                var file = new StreamReader(mapTextFileName);

                while ((readedText = file.ReadLine()) != null)
                {
                    loadedMap.Add(readedText);
                }

                file.Close();
            }
            else
            {
                MessageBox.Show("There is no : (" + mapTextFileName + ") in current program directory. \nPlease creates one with all map you want and restart this program",
                    "Opps..",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }

            dice = new Random();

            for (int i = 0; i < MAX_SERVER; i++)
            {
                var indexInitMap = 0;
                var initMap = "";

                if ((i + 1) == 10)
                {
                    initMap = "nmo_dodgeball_v5";
                }
                else
                {
                    if ((i + 1) == 9)
                        indexInitMap = dice.Next(loadedMap.Count);
                    else
                        indexInitMap = dice.Next(10);

                    initMap = loadedMap[indexInitMap];
                }

                Console.WriteLine("Result gen map Server ({0}) : {1}", (i + 1).ToString(), initMap);

                //"srcds.exe -game nmrih -insecure -console -nogui -port 28010 +exec server1.cfg +map nmo_fema";
                var commandArgs = COMMON_COMMAND_ARGS;
                commandArgs += " -port " + (INIT_PORT + i);
                commandArgs += " +exec " + "server" + (i + 1) + ".cfg";
                commandArgs += " +map " + initMap;           // <- Change to random map later

                Console.WriteLine("Result Command Server ({0}) : {1} {2}", (i + 1).ToString(), COMMAND, commandArgs);

                var info = new ServerInfo(server_names[i],
                    HOST,
                    INIT_PORT + i,  //Actual is 28010
                    COMMAND,
                    commandArgs,
                    initMap);

                // No-Gui
                // (fuck window) who need one?
                //info.ProcessStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // Minimized 
                //(for able to maximize any time and doesn't appear on the first top window when server's author is playing game.)
                //It still stole window focus though.. -> Consider using hidden style :)
                info.ProcessStartInfo.WindowStyle = ProcessWindowStyle.Minimized;

                servers.Add(new Server(info));
                clients.Add(new Client());
            }

            var delay = 0;
            foreach (Server server in servers)
            {
                Task.Delay(delay).ContinueWith(t => server.Start());
                delay += START_SERVER_DELAY;
            }
        }

        // This should be in 'Setting'
        private void LoadConfig()
        {

        }

        private void InitView()
        {
            for (int i = 0; i < MAX_SERVER; i++)
            {
                var viewItem = new ListViewItem();
                viewItem.Text = "#" + (i + 1);
                viewItem.SubItems.Add(servers[i].Info.Name);
                viewItem.SubItems.Add(servers[i].Info.Port.ToString());
                viewItem.SubItems.Add("Unknown");
                viewItem.SubItems.Add("Unknown");
                viewItem.SubItems.Add("Unknown");
                lstServerStat.Items.Add(viewItem);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;

            var result = MessageBox.Show("Quit and close all server?",
                "Quit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (DialogResult.Yes == result)
            {
                foreach (Server server in servers)
                {
                    server.Stop();
                }
                e.Cancel = false;
            }
        }
    }
}