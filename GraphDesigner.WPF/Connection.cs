using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Data;

namespace HFK.GraphDesigner.WPF
{
    public class Connection : Shape
    {
        public Connection()
        {
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 1;
        }

        public static readonly DependencyProperty SourceConnectionPointProperty =
            DependencyProperty.Register("SourceConnectionPoint", 
                typeof(ConnectionPoint), 
                typeof(Connection),
                new FrameworkPropertyMetadata(null, 
                    new PropertyChangedCallback(OnSourceConnectionPointPropertyChanged)));

        private static void OnSourceConnectionPointPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Connection connection = d as Connection;

            Binding startPointBinding = new Binding("ConnectAt");
            startPointBinding.Source = e.NewValue as ConnectionPoint;

            connection.SetBinding(Connection.StartPointProperty, startPointBinding);
        }

        public ConnectionPoint SourceConnectionPoint
        {
            get
            {
                return (ConnectionPoint)GetValue(SourceConnectionPointProperty);
            }
            set
            {
                SetValue(SourceConnectionPointProperty, value);
            }

        }

        public static readonly DependencyProperty TargetConnectionPointProperty =
            DependencyProperty.Register("TargetConnectionPoint",
                typeof(ConnectionPoint),
                typeof(Connection),
                new FrameworkPropertyMetadata(null,
                    new PropertyChangedCallback(OnTargetConnectionPointPropertyChanged)));

        private static void OnTargetConnectionPointPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Connection connection = d as Connection;

            Binding endPointBinding = new Binding("ConnectAt");
            endPointBinding.Source = e.NewValue as ConnectionPoint;

            connection.SetBinding(Connection.EndPointProperty, endPointBinding);
        }

        public ConnectionPoint TargetConnectionPoint
        {
            get
            {
                return (ConnectionPoint)GetValue(TargetConnectionPointProperty);
            }
            set
            {
                SetValue(TargetConnectionPointProperty, value);
            }

        }

        private Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        private static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint",
                                         typeof(Point),
                                         typeof(Connection),
                                         new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        private Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        private static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint",
                                         typeof(Point),
                                         typeof(Connection),
                                         new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        protected override Geometry DefiningGeometry
        {
            get
            {
                linegeo.StartPoint = StartPoint;
                linegeo.EndPoint = EndPoint;
                return linegeo;
            }
        }

        private LineGeometry linegeo = new LineGeometry();
    }
}
