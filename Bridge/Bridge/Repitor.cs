using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Threading;

namespace Bridge
{
    class Repitor
    {
        public Network_Bus[] Ports;
        List<RecordMAC>[] MACListPorts;
        int portsCount = 2;
        public bool power = true;
        int timeWork = 0;

        public Repitor(int Nports = 2)
        {

            portsCount = Nports;
            Ports = new Network_Bus[portsCount];
            MACListPorts = new List<RecordMAC>[portsCount];
        }

        public void AddNetwork_Bus(Network_Bus network_Bus, int port)
        {
            network_Bus.RegisterHandlerSendingNetBus(Listen);
            network_Bus.RegisterHandlerwithOutMost(Listen);
            Ports[port] = network_Bus;
        }

        public delegate void SendingToTerminal(string message);
        SendingToTerminal sendToTerminal;
        public void RegisterHandlersendToTerminal(SendingToTerminal del)
        {
            sendToTerminal += del;
        }

        public delegate void ReColor();
        ReColor reColor;
        public void RegisterHandlerReColor(ReColor del)
        {
            reColor += del;
        }

        void SendToPort(string macTo, string macFrom, string ipTo, string ipFrom, string message, int port)
        {
            Ports[port].Listen(macTo, macFrom, ipTo, ipFrom, message);
            Ports[port].SendWithOutOneBridge(macTo, macFrom, ipTo, ipFrom, message, this, Ports[port]);
        }

        void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message, object e)
        {
            if (power)
            {
                System.Threading.Thread.Sleep(3);
                sendToTerminal("\nPackage on repitor");
                if (e == Ports[0])
                    SendToPort(macTo, macFrom, ipTo, ipFrom, message, 1);
                else
                    SendToPort(macTo, macFrom, ipTo, ipFrom, message, 0);
            }
        }

        void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message, object bridge, object bus)
        {
            if (power && bridge != this)
            {
                timeWork++;
                if (timeWork > 5)
                {
                    power = false;
                    timeWork = 0;
                    reColor();
                }
                System.Threading.Thread.Sleep(3);
                sendToTerminal("\nPackage on repitor");
                if (bus == Ports[0])
                    SendToPort(macTo, macFrom, ipTo, ipFrom, message, 1);
                else
                    SendToPort(macTo, macFrom, ipTo, ipFrom, message, 0);
            }
        }

    }
}