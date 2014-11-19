using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace HFK.GraphDesigner.WPF
{

    public class GraphNode : ContentItemsControl
    {
        public event EventHandler<EventArgs> NodesGenerated;

        static GraphNode()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphNode), new FrameworkPropertyMetadata(typeof(GraphNode)));
        }

        public GraphNode()
        {
            connections = new ObservableCollection<Connection>();
            connections.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(Connections_CollectionChanged);

            ItemContainerGenerator.StatusChanged += new EventHandler(ItemContainerGenerator_StatusChanged);
        }

        private void ItemContainerGenerator_StatusChanged(object sender, EventArgs e)
        {
            ItemContainerGenerator generator = sender as ItemContainerGenerator;
            if (generator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
            {
                if (NodesGenerated != null)
                {
                    NodesGenerated(this, new EventArgs());
                }
            }
        }

        public static readonly DependencyProperty PosXProperty =
            DependencyProperty.Register("PosX", typeof(double), typeof(GraphNode));

        public double PosX
        {
            get
            {
                return (double)GetValue(PosXProperty);
            }
            set
            {
                SetValue(PosXProperty, value);
            }
        }

        public static readonly DependencyProperty PosYProperty =
            DependencyProperty.Register("PosY", typeof(double), typeof(GraphNode));

        public double PosY
        {
            get
            {
                return (double)GetValue(PosYProperty);
            }
            set
            {
                SetValue(PosYProperty, value);
            }
        }

        public GraphDesigner Diagram
        {
            get
            {
                if (graph == null)
                {
                    DependencyObject element = this;
                    while (element != null && !(element is GraphDesigner))
                        element = VisualTreeHelper.GetParent(element);
                    graph = element as GraphDesigner;
                }

                return graph;
            }
        }

        public ObservableCollection<Connection> Connections
        {
            get
            {
                return connections;
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ConnectionPoint();
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is ConnectionPoint)
            {
                ConnectionPoint connectionPoint = element as ConnectionPoint;
                if (connectionPoint.Content == null && connectionPoint.ContentTemplate == null)
                {
                    connectionPoint.ContentTemplate = ItemTemplate as DataTemplate;
                }

                if (!(item is ConnectionPoint))
                {
                    if (Diagram.ItemTemplate is GraphDataTemplate)
                    {
                        GraphDataTemplate graphDataTemplate = Diagram.ItemTemplate as GraphDataTemplate;
                        if(!string.IsNullOrEmpty(graphDataTemplate.Docking))
                        {
                            Dock docking;
                            object dockingAsObject = graphDataTemplate.GetDocking(item);
                            if(dockingAsObject.GetType() != typeof(Dock))
                            {
                                if(graphDataTemplate.DockingConverter == null)
                                {
                                }

                                docking = (Dock)graphDataTemplate.DockingConverter.Convert(dockingAsObject, typeof(Dock), null, null);
                            }
                            else
                            {
                                docking = (Dock)dockingAsObject;
                            }

                            MultiDockPanel.SetDock(connectionPoint, docking);
                        }
                    }
                }
            }
        }


        void Connections_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                if (Diagram != null)
                {
                    Diagram.AddConnection(e.NewItems);
                }
            }
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                if (Diagram != null)
                {
                    Diagram.RemoveConnection(e.OldItems);
                }
            }
        }

        ObservableCollection<Connection> connections;
        GraphDesigner graph;
    }
}
