using System;
using System.Collections.Generic;
using System.Windows.Threading;

namespace Bridge
{
    class Most
    {
        static int count=0;
        int portsCount;
        public Network_Bus[] Ports;
        List<RecordMAC>[] MACListPorts;
        DispatcherTimer timer = new DispatcherTimer();
        int timeWork;
        int timeToLive;
        int id;

        public delegate void SendingToTerminal(string message);
        SendingToTerminal sendToTerminal;
        public void RegisterHandlersendToTerminal(SendingToTerminal del)
        {
            sendToTerminal += del;
        }

        public Most(int Nports=2)
        {
            count++;
            id = count;
            portsCount = Nports;
            Ports = new Network_Bus[portsCount];
            MACListPorts = new List<RecordMAC>[portsCount];
            for (int i = 0; i < portsCount; i++)
                MACListPorts[i] = new List<RecordMAC>();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
            timeWork = 0;
            timeToLive = 30;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timeWork++;
            Delrecord(timeWork);
        }

        void Delrecord (int timeNow)
        {
            for (int port = 0; port < portsCount; port++)
            {
                int record = 0;
                for (record = 0; record < MACListPorts[port].Count; record++)
                {
                    if (timeWork - MACListPorts[port][record].timeToUpdate > timeToLive)
                    {
                        MACListPorts[port].Remove(MACListPorts[port][record]);
                        record--;
                    }    
                }               
            }
        }

        public void AddNetwork_Bus(Network_Bus network_Bus, int port)
        {
            network_Bus.RegisterHandlerSendingNetBus(Listen);
            network_Bus.RegisterHandlerwithOutMost(Listen);
            Ports[port] = network_Bus;
        }

        void SendAll(string macTo, string macFrom, string ipTo, string ipFrom, string message, int noPort)
        {
            for (int i = 0; i < portsCount; i++)
            {
                if (i != noPort && Ports[i] != null)
                {
                    Ports[i].Listen(macTo, macFrom, ipTo, ipFrom, message);
                    if (message == "broadcast")
                        Ports[i].SendWithOutOneBridge(macTo, macFrom, ipTo, ipFrom, message, this, Ports[i]);
                }    
            }
        }

        void SendToPort(string macTo, string macFrom, string ipTo, string ipFrom, string message, int port)
        {
            Ports[port].Listen(macTo, macFrom, ipTo, ipFrom, message);
            Ports[port].SendWithOutOneBridge(macTo, macFrom, ipTo, ipFrom, message,this, Ports[port]);
        }

        void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message, object bridge, object bus)
        {
            if (bridge != this)
            {
                System.Threading.Thread.Sleep(3);
                sendToTerminal("\nPackage on brudge №" + id);
                int i;
                for (i = 0; i < portsCount && bus != Ports[i]; i++) ;
                int noPort = i;
                int port = SearchMAC(macTo, noPort);
                i = 0;
                while (i < MACListPorts[noPort].Count && (macFrom != MACListPorts[noPort][i].MAC))
                {
                    i++;
                }
                if (i == MACListPorts[noPort].Count)
                    MACListPorts[noPort].Add(new RecordMAC(timeWork, macFrom));
                else
                    MACListPorts[noPort][i].UpdateIP(timeWork);
                if (port != -2)
                {
                    if (port == -1)
                    {
                        SendAll(macTo, macFrom, ipTo, ipFrom, message, noPort);
                    }
                    else
                        SendToPort(macTo, macFrom, ipTo, ipFrom, message, port);
                }
            }
        }

        void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message, object e)
        {
            System.Threading.Thread.Sleep(3);
            sendToTerminal("\nPackage on brudge №" + id);
            int i;
            for (i = 0; i < portsCount && e != Ports[i]; i++);
            int noPort = i;
            int port = SearchMAC(macTo, noPort);
            i = 0;
            while (i < MACListPorts[noPort].Count && (macFrom != MACListPorts[noPort][i].MAC))
            {
                i++;
            }

            if (i == MACListPorts[noPort].Count)
                MACListPorts[noPort].Add(new RecordMAC(timeWork, macFrom));
            else
                MACListPorts[noPort][i].UpdateIP(timeWork);

            if (port != -2)
            {
                if (port == -1)
                {
                    SendAll(macTo, macFrom, ipTo, ipFrom, message, noPort);
                }
                else
                    SendToPort(macTo, macFrom, ipTo, ipFrom, message, port);
            } 
        }

        int SearchMAC(string MAC, int noPort)
        {
            for (int port = 0; port < portsCount; port++)
            {
                if (port != noPort)
                {
                    foreach (RecordMAC _MAC in MACListPorts[port])
                    {
                        if (MAC == _MAC.MAC)
                            return port;
                    }
                }
                else
                {
                    foreach (RecordMAC _MAC in MACListPorts[port])
                    {
                        if (MAC == _MAC.MAC)
                            return -2;
                    }
                }
            }
            return -1;
        }

        public string PrintTable()
        {
            string output = "Bridge №"+id+" run time : "+timeWork+"\n";
            for (int i = 0; i < MACListPorts.Length; i++)
            {
                output += "Port " + i + " :\n";
                output += "|MAC-address     |Time to Updata|\n";
                foreach (RecordMAC rm in MACListPorts[i])
                {
                    output += "|" + rm.MAC +"| "+rm.timeToUpdate+ "\n";
                }
                output += "-------------------|---------------\n";
            }
            return output;
        }
    }
}