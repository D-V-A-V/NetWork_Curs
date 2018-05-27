using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Bridge
{
    class PC_View
    {
        Rectangle myRect;
        Rectangle trigger;
        Grid gridDraw;
        protected SolidColorBrush blackBrush
            = new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) };
        Point location;
        int size = 20;
        PC_Client _Client;

        public delegate void  Choose(PC_Client pC_Client);

        Choose choose;
        public void RegisterHandlerChoose(Choose del)
        {
            choose = del;
        }

        public PC_View(Point loc, Grid grid, PC_Client pC_Client)
        {
            gridDraw = grid;
            location = loc;
            _Client = pC_Client;
        }

        public void CreatePC()
        {
            myRect = new Rectangle
            {
                Stroke = blackBrush,
                Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)),
                Margin = new Thickness(location.X+size/2, 0, 0, location.Y + size / 2),
                Height = size,
                Width = size,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            trigger = new Rectangle
            {
                Fill = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255)),
                Margin = new Thickness(location.X + size / 2, 0, 0, location.Y + size / 2),
                Height = size,
                Width = size,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };

           
            gridDraw.Children.Add(myRect);
            gridDraw.Children.Add(trigger);

            trigger.ToolTip = "IP : " + _Client.IP + "\n" + "MAC : " + _Client.MAC_Address;
            trigger.MouseLeftButtonDown += Trigger_MouseLeftButtonDown;
        }

        private void Trigger_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            choose(_Client);
        }
    }



}