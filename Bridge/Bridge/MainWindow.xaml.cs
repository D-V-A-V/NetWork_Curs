using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Bridge
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        PC_View[] pC_Clients;
        Most_View[] most_View;
        Most most1, most2;
        PC_Client choosenClient;
        Repitor repitor;
        Repitor_View repitor_View;
        public MainWindow()
        {
            most1 = new Most();
            most2 = new Most();
            repitor = new Repitor();
            Network_Bus Network_Bus1 = new Network_Bus();
            Network_Bus Network_Bus2 = new Network_Bus();
            Network_Bus Network_Bus3 = new Network_Bus();

            InitializeComponent();

            Network_Bus1.RegisterHandlersendToTerminal(Show_Message);
            Network_Bus2.RegisterHandlersendToTerminal(Show_Message);
            Network_Bus3.RegisterHandlersendToTerminal(Show_Message);

            most1.RegisterHandlersendToTerminal(Show_Message);
            most2.RegisterHandlersendToTerminal(Show_Message);
            repitor.RegisterHandlersendToTerminal(Show_Message);

            for (int i = 0; i < 2; i++)
            {
                Network_Bus1.Add(new PC_Client("192.168.1." + (i+1).ToString()));
                Network_Bus1.pC_Clients[i].RegisterHandlersendToTerminal(Show_Message);
            }
            for (int i = 0; i < 2; i++)
            {
                Network_Bus2.Add(new PC_Client("192.168.1." + (i + 3).ToString()));
                Network_Bus2.pC_Clients[i].RegisterHandlersendToTerminal(Show_Message);
            }
            for (int i = 0; i < 2; i++)
            {
                Network_Bus3.Add(new PC_Client("192.168.1." + (i + 10).ToString()));
                Network_Bus3.pC_Clients[i].RegisterHandlersendToTerminal(Show_Message);
            }

            most1.AddNetwork_Bus(Network_Bus1, 0);
            most1.AddNetwork_Bus(Network_Bus2, 1);
            most2.AddNetwork_Bus(Network_Bus2, 0);
            most2.AddNetwork_Bus(Network_Bus3, 1);
            repitor.AddNetwork_Bus(Network_Bus1, 0);
            repitor.AddNetwork_Bus(Network_Bus3, 1);

            DrawBus();

            pC_Clients = new PC_View[6];
            pC_Clients[0] = new PC_View(new Point(30, 140), grid, Network_Bus1.pC_Clients[0]);
            pC_Clients[1] = new PC_View(new Point(30, 90), grid, Network_Bus1.pC_Clients[1]);
            pC_Clients[2] = new PC_View(new Point(110, 140), grid, Network_Bus2.pC_Clients[0]);
            pC_Clients[3] = new PC_View(new Point(110, 90), grid, Network_Bus2.pC_Clients[1]);
            pC_Clients[4] = new PC_View(new Point(200, 140), grid, Network_Bus3.pC_Clients[0]);
            pC_Clients[5] = new PC_View(new Point(200, 90), grid, Network_Bus3.pC_Clients[1]);

            foreach (PC_View pc in pC_Clients)
            {
                pc.CreatePC();
                pc.RegisterHandlerChoose(ChoosePC);   
            }
            choosenClient = Network_Bus1.pC_Clients[0];

            most_View = new Most_View[2];
            most_View[0] = new Most_View(new Point(90, 350+270),grid, most1);
            most_View[1] = new Most_View(new Point(175, 350 + 270), grid, most2);
            most_View[0].CreatePC();
            most_View[1].CreatePC();

            repitor_View = new Repitor_View(new Point(175, 60), grid, repitor);
            repitor_View.CreatePC();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            BridgeTable1.Text = most1.PrintTable() + "\n" + most2.PrintTable();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Log.Text += " Ping from " + choosenClient.IP + " to " + ipToTB.Text + " :\n";
            choosenClient.Send("", ipToTB.Text, "ping");
        }

        void ChoosePC(PC_Client _Client)
        {
            choosenClient = _Client;
            lb1.Content = "Ping from "+choosenClient.IP+" to";
        }

        private void Show_Message(String message)
        {
            Log.Text += " " + message + "\n";
        }

        private void DrawBus()
        {
            Line line1 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 10,
                X2 = 275,
                Y1 = 350 + 275,
                Y2 = 350 + 275

            };
            Line line2 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 50,
                X2 = 50,
                Y1 = 350 + 250,
                Y2 = 350 + 300

            };
            Line line3 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 130,
                X2 = 130,
                Y1 = 350 + 250,
                Y2 = 350 + 300

            };
            Line line4 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 220,
                X2 = 220,
                Y1 = 350 + 250,
                Y2 = 350 + 300

            };

            Line line5 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 10,
                X2 = 275,
                Y1 = 350 + 330,
                Y2 = 350 + 330

            };

            Line line6 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 10,
                X2 = 10,
                Y1 = 350 + 330,
                Y2 = 350 + 275

            };
            Line line7 = new Line
            {
                Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
                X1 = 275,
                X2 = 275,
                Y1 = 350 + 330,
                Y2 = 350 + 275

            };

            grid.Children.Add(line1);
            grid.Children.Add(line2);
            grid.Children.Add(line3);
            grid.Children.Add(line4);
            grid.Children.Add(line5);
            grid.Children.Add(line6);
            grid.Children.Add(line7);
        }
    }
}
