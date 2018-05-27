using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bridge
{
    class PC_Client
    {
        static int count = 0;
        string ip_address;
        public string IP {
            set {
                bool isCorect = true;
                if (value.Length < 16)
                {
                    int count=0;
                    for (int i = 0; i < 4; i++)
                    {
                        int j = count;
                        while ((j < value.Length) && (value[j] != '.')) 
                            j++;
                        
                        if ((j == value.Length-1) && (i != 3))
                        {
                            isCorect = false;
                            break;
                        } else
                        {
                            string temp = "";
                            for (int k = count; k < j; k++)
                            {
                                temp += value[k].ToString();
                            }
                            if (Int32.TryParse(temp, out int a))
                            {
                                if (a > 255)
                                {
                                    isCorect = false;
                                    break;
                                }
                            }
                        }
                        count = j + 1;
                    }
                }else
                    isCorect = false;
                if (isCorect)
                    ip_address = value;
                else
                    ip_address = "0.0.0.0";

            }
            get { return ip_address; }
        }
        string mac_adress;
        public string MAC_Address {
            set { mac_adress = value; }
            get { return mac_adress; }
        }
        public Dictionary<string, string> IpToMac = new Dictionary<string, string>();

        public delegate void Sending(string macTo, string macFrom, string ipTo, string ipFrom, string message);

        public delegate void SendingToTerminal(string message);

        SendingToTerminal sendToTerminal;
        Sending SendingPC;

        public void RegisterHandlersendToTerminal(SendingToTerminal del)
        {
            sendToTerminal = del;
        }

        public void RegisterHandlerSendingPC(Sending del)
        {
            SendingPC += del;
        }

        public PC_Client(string ip = "0.0.0.0")
        {
            IP = ip;
            count++;
            if (count < 10)
            {
                mac_adress = "00:aa:00:64:c8:0" + count;
            }
            else if (count < 20)
            {
                mac_adress = "00:aa:00:64:c8:a" + count % 10;
            }
            else if (count < 30)
            {
                mac_adress = "00:aa:00:64:c8:b" + count % 10;
            }

        }

        public void Send(string macTo, string ipTo, string message) {
            if (!IpToMac.ContainsKey(ipTo))
            {
                sendToTerminal("\nSEND message: broadcast:\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP to: " + ipTo + "\n" + "MAC to: " + macTo);
                SendingPC(macTo, mac_adress, ipTo, IP, "broadcast");
            }
            if (!IpToMac.ContainsKey(ipTo))
            {
                sendToTerminal("no Replay");
            }else
            {
                sendToTerminal("\nSEND message: " + message + ":\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP to: " + ipTo + "\n" + "MAC to: " + IpToMac[ipTo]);
                SendingPC(IpToMac[ipTo], mac_adress, ipTo, IP, message);
            }
            
        }

        public void Listen(string macTo, string macFrom, string ipTo, string ipFrom, string message)
        {
            System.Threading.Thread.Sleep(3);
            switch (message)
            {
                case "broadcast":
                    if (IP == ipTo)
                    {
                        if (!IpToMac.ContainsKey(ipFrom))
                            IpToMac.Add(ipFrom, macFrom);
                        sendToTerminal("\nGET message: " + message + ":\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP from: " + ipFrom + "\n" + "MAC from: " + macFrom);
                        Send(macFrom, ipFrom, "broadcast_report");
                    }                  
                    break;
                case "broadcast_report":
                    if (IP == ipTo)
                    {
                        sendToTerminal("\nGET message: " + message + ":\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP from: " + ipFrom + "\n" + "MAC from: " + macFrom);
                        if (!IpToMac.ContainsKey(ipFrom))
                            IpToMac.Add(ipFrom, macFrom);
                    }
                    break;
                case "ping":
                    if (IP == ipTo)
                    {
                        sendToTerminal("\nGET message: " + message + ":\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP from: " + ipFrom + "\n" + "MAC from: " + macFrom);
                        Send(macFrom, ipFrom, "pong");
                    }
                    break;
                case "pong":
                    if (IP == ipTo)
                    {
                        sendToTerminal("\nGET message: " + message + ":\n" + "IP : " + IP + "\n" + "MAC : " + MAC_Address + "\n" +
                            "IP from: " + ipFrom + "\n" + "MAC from: " + macFrom);
                        sendToTerminal("Replay");
                    }
                    break;
            }

        }
    }
}
