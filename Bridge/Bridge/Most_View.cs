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
    class Most_View
    {
        Polygon myPolygon;
        Grid gridDraw;
        protected SolidColorBrush blackBrush
            = new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) };
        Point location;
        int size = 20;
        Most most;

        public delegate void Choose(PC_Client pC_Client);

        Choose choose;
        public void RegisterHandlerChoose(Choose del)
        {
            choose = del;
        }

        public Most_View(Point loc, Grid grid, Most most)
        {
            gridDraw = grid;
            location = loc;
            this.most = most;
        }

        public void CreatePC()
        {
            myPolygon = new Polygon
            {
                Stroke = blackBrush,
                Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 255)),
                Points = { new Point(location.X- size / 2, location.Y + size / 2), new Point(location.X + size / 2, location.Y + size / 2),
                    new Point(location.X, location.Y - size / 2) },

                HorizontalAlignment = HorizontalAlignment.Left
            };
            gridDraw.Children.Add(myPolygon);
        }

       // private void Trigger_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
       // {
      //      choose(_Client);
      //  }
    }



}