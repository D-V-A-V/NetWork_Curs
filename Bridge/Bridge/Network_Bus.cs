using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    class Network_Bus
    {
        static int count = 0;
        int id;
        public List<PC_Client> pC_Clients = new List<PC_Client>();

        public delegate void SendingToTerminal(string message);
        SendingToTerminal sendToTerminal;
        public void RegisterHandlersendToTerminal(SendingToTerminal del)
        {
            sendToTerminal += del;
        }

        public delegate void Sending(string macTo, string macFrom, string ipTo, string ipFrom, string message, object e);
        Sending SendingNetBus;
        public void RegisterHandlerSendingNetBus(Sending del)
        {
            SendingNetBus += del;
        }

        public delegate void WithOutMost(string macTo, string macFrom, string ipTo, string ipFrom, string message, object bridge, object bus);
        WithOutMost withOutMost;
        public void RegisterHandlerwithOutMost(WithOutMost del)
        {
            withOutMost += del;
        }

        public Network_Bus()
        {
            count++;
            id = count;
        }

        public void Add(PC_Client pC_Client)
        {
            pC_Client.RegisterHandlerSendingPC(Send);
            pC_Clients.Add(pC_Client);
        }

        public void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message) {
            System.Threading.Thread.Sleep(3);
            sendToTerminal("\nPackage on network bus №" + id);
            foreach (PC_Client p_c in pC_Clients)
                p_c.Listen(macTo, macFrom, ipTo, ipFrom, message);
        }

        public void SendWithOutOneBridge(string macTo, string macFrom, string ipTo, string ipFrom, string message, object bridge, object bus)
        {
            withOutMost(macTo, macFrom, ipTo, ipFrom, message, bridge,bus);
        }

        private void Send(string macTo, string macFrom, string ipTo, string ipFrom, string message)
        {
            Listen(macTo, macFrom, ipTo, ipFrom, message);
            SendingNetBus(macTo, macFrom, ipTo, ipFrom, message, this);
        }
    }
}
