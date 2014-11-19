using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Input;

namespace HFK.GraphDesigner.WPF
{
    public class ConnectionAdorner : Adorner
    {
        public ConnectionAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            drawingPen = new Pen(Brushes.LightSlateGray, 1);
            drawingPen.LineJoin = PenLineJoin.Round;
        }

        public Point StartPoint
        {
            get { return startPoint; }
            set
            {
                startPoint = value;
                InvalidateVisual();
            }
        }

        public Point EndPoint
        {
            get { return endPoint; }
            set
            {
                endPoint = value;
                InvalidateVisual();
            }
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);
            dc.DrawLine(drawingPen, StartPoint, EndPoint);
        }

        private Pen drawingPen;
        private Point startPoint;
        private Point endPoint;
    }
}

