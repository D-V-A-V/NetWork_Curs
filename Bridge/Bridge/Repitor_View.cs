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
    class Repitor_View
    {
        Rectangle myRect;
        Grid gridDraw;
        protected SolidColorBrush blackBrush
            = new SolidColorBrush() { Color = Color.FromArgb(255, 0, 0, 0) };
        Point location;
        int size = 20;
        Repitor repitor;

        public Repitor_View(Point loc, Grid grid, Repitor _repitor)
        {
            gridDraw = grid;
            location = loc;
            repitor = _repitor;
            repitor.RegisterHandlerReColor(Recolor);
        }

        public void CreatePC()
        {
            myRect = new Rectangle
            {
                Stroke = blackBrush,
                Fill = new SolidColorBrush(Color.FromArgb(255, 255, 25, 255)),
                Margin = new Thickness(location.X + size / 2,0, 0, location.Y + size / 2),
                Height = size,
                Width = size,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            gridDraw.Children.Add(myRect);

            myRect.MouseLeftButtonDown += Trigger_MouseLeftButtonDown;
        }

        void Recolor()
        {
            if (repitor.power == false)
            {
                myRect.ToolTip = "Off";
                myRect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            }
            else
            {
                myRect.ToolTip = "On";
                myRect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 25, 255));
            }
        }

        private void Trigger_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            repitor.power = !repitor.power;
            if (repitor.power == false)
            {
                myRect.ToolTip = "Off";
                myRect.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            }
            else
            {
                myRect.ToolTip = "On";
                myRect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 25, 255));
            }
                
        }
    }



}