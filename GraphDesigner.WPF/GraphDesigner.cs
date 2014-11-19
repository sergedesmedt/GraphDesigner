using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using HFK.GraphDesigner.WPF.Services;
using System.Collections.Specialized;

namespace HFK.GraphDesigner.WPF
{
    public class GraphDesigner : ItemsControl
    {
        static GraphDesigner()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphDesigner), new FrameworkPropertyMetadata(typeof(GraphDesigner)));
        }

        public GraphDesigner()
        {
            SelectionService = new SelectionService<GraphDesigner>(this);
        }

        protected override bool  IsItemItsOwnContainerOverride(object item)
        {
            return item is GraphNode;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GraphNode();
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);

            List<object> connectionDataSelectedForRemoval = new List<object>();
            List<Connection> connectionsSelectedForRemoval = new List<Connection>();
            if (element is GraphNode)
            {
                GraphNode node = element as GraphNode;

                if (ItemTemplate is GraphDataTemplate)
                {
                    GraphDataTemplate graphDataTemplate = ItemTemplate as GraphDataTemplate;
                    foreach (object connnectionPointAsData in node.ItemsSource)
                    {
                        if (pointConnectionMap.ContainsKey(connnectionPointAsData))
                        {
                            List<object> connectionList = pointConnectionMap[connnectionPointAsData];
                            foreach (object connectionAsData in connectionList)
                            {
                                ConnectionState connectionState = connectionStateMap[connectionAsData];

                                connectionsSelectedForRemoval.Add(connectionState.TheConnection);
                                connectionDataSelectedForRemoval.Add(connectionAsData);
                            }
                        }

                    }

                    foreach (Connection connection in GetConnectionPanel().Children)
                    {
                        if ((node.Items.Contains(graphDataTemplate.GetStartPoint(connection.DataContext))
                            || node.Items.Contains(graphDataTemplate.GetStartPoint(connection.DataContext)))
                        && !connectionsSelectedForRemoval.Contains(connection))
                        {
                            connectionsSelectedForRemoval.Add(connection);
                        }
                    }

                    node.NodesGenerated -= new EventHandler<EventArgs>(node_NodesGenerated);
                }
                else
                {
                    foreach (Connection connection in GetConnectionPanel().Children)
                    {
                        if ((node.Items.Contains(connection.SourceConnectionPoint)
                            || node.Items.Contains(connection.TargetConnectionPoint))
                        && !connectionsSelectedForRemoval.Contains(connection))
                        {
                            connectionsSelectedForRemoval.Add(connection);
                        }
                    }
                }
            }

            RemoveConnection(connectionsSelectedForRemoval);
            RemoveConnectionAsData(connectionsSelectedForRemoval);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            if (element is GraphNode)
            {
                GraphNode node = element as GraphNode;

                Binding posXBinding = new Binding("PosX");
                posXBinding.Source = node;
                posXBinding.Mode = BindingMode.TwoWay;
                node.SetBinding(Canvas.LeftProperty, posXBinding);

                Binding posYBinding = new Binding("PosY");
                posYBinding.Source = node;
                posYBinding.Mode = BindingMode.TwoWay;
                node.SetBinding(Canvas.TopProperty, posYBinding);

                if (ItemTemplate != null)
                {
                    ContentItemsControl nodeAsContentItemsControl = element as ContentItemsControl;
                    nodeAsContentItemsControl.Content = item;

                    if (ItemTemplate is GraphDataTemplate)
                    {
                        GraphDataTemplate graphDataTemplate = ItemTemplate as GraphDataTemplate;

                        Binding posXDataBinding = new Binding(graphDataTemplate.XPos);
                        posXDataBinding.Source = item;
                        posXDataBinding.Mode = BindingMode.TwoWay;
                        node.SetBinding(GraphNode.PosXProperty, posXDataBinding);

                        Binding posYDataBinding = new Binding(graphDataTemplate.YPos);
                        posYDataBinding.Source = item;
                        posYDataBinding.Mode = BindingMode.TwoWay;
                        node.SetBinding(GraphNode.PosYProperty, posYDataBinding);

                        node.SetBinding(GraphNode.ItemsSourceProperty, graphDataTemplate.ItemsSource);
                        nodeAsContentItemsControl.SetValue(ContentItemsControl.ItemTemplateProperty, graphDataTemplate.ItemTemplate);
                        nodeAsContentItemsControl.SetValue(ContentItemsControl.ContentTemplateProperty, ItemTemplate);

                        IEnumerable connectionListAsEnumerable = graphDataTemplate.GetConnections(item);

                        if (connectionListAsEnumerable == null)
                        {
                        }

                        SetConnectionType(graphDataTemplate.GetConnectionsType(item));

                        if (!(connectionListAsEnumerable is INotifyCollectionChanged))
                        {
                        }

                        foreach (object connectionAsData in connectionListAsEnumerable)
                        {
                            AddConnectionAsData(connectionAsData);
                        }

                        INotifyCollectionChanged connectionListAsNotifyable = connectionListAsEnumerable as INotifyCollectionChanged;
                        connectionListAsNotifyable.CollectionChanged += new NotifyCollectionChangedEventHandler(connectionListAsNotifyable_CollectionChanged);

                        node.NodesGenerated += new EventHandler<EventArgs>(node_NodesGenerated);
                    }
                    else
                    {
                        nodeAsContentItemsControl.SetValue(ContentItemsControl.ContentTemplateProperty, ItemTemplate);

                        if (node.Connections != null)
                        {
                            AddConnection(node.Connections);
                        }
                    }
                }
                else
                {
                    if (node.Connections != null)
                    {
                        AddConnection(node.Connections);
                    }
                }
            }
        }

        private void connectionListAsNotifyable_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!reportConnectionAdded)
                return;

            GraphDataTemplate graphDataTemplate = ItemTemplate as GraphDataTemplate;

            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                List<Connection> connectionsToAdd = new List<Connection>();
                foreach (object connectionAsData in e.NewItems)
                {
                    Connection connection = new Connection();
                    foreach (object nodeAsData in ItemsSource)
                    {
                        GraphNode node = ItemContainerGenerator.ContainerFromItem(nodeAsData) as GraphNode;
                        foreach (object pointAsData in node.ItemsSource)
                        {
                            if (pointAsData == graphDataTemplate.GetStartPoint(connectionAsData))
                            {
                                connection.SourceConnectionPoint = node.ItemContainerGenerator.ContainerFromItem(pointAsData) as ConnectionPoint; 
                            }

                            if (pointAsData == graphDataTemplate.GetEndPoint(connectionAsData))
                            {
                                connection.TargetConnectionPoint = node.ItemContainerGenerator.ContainerFromItem(pointAsData) as ConnectionPoint;
                            }
                        }
                    }

                    if (connection.SourceConnectionPoint == null && connection.TargetConnectionPoint == null)
                    {
                        continue;
                    }

                    connection.DataContext = connectionAsData;
                    connectionsToAdd.Add(connection);
                }

                AddConnection(connectionsToAdd);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                List<Connection> connectionsToRemove = new List<Connection>();
                foreach (object connectionAsData in e.OldItems)
                {
                    foreach (Connection connection in GetConnectionPanel().Children)
                    {
                        if (connection.DataContext == connectionAsData)
                            connectionsToRemove.Add(connection);
                    }
                }

                RemoveConnection(connectionsToRemove);
            }
        }

        private void node_NodesGenerated(object sender, EventArgs e)
        {
            if (ItemTemplate is GraphDataTemplate)
            {
                GraphNode node = sender as GraphNode;

                if (node.ItemsSource == null)
                {
                    return;
                }

                foreach (object connnectionPointAsData in node.ItemsSource)
                {
                    object connectionPointAsObject = node.ItemContainerGenerator.ContainerFromItem(connnectionPointAsData);
                    ConnectionPoint connectionPoint = connectionPointAsObject as ConnectionPoint;

                    if (!pointConnectionMap.ContainsKey(connnectionPointAsData))
                    {
                        continue;
                    }

                    List<object> connectionList = pointConnectionMap[connnectionPointAsData];

                    List<object> selectedForRemoval = new List<object>();
                    foreach (object connectionAsData in connectionList)
                    {
                        ConnectionState connectionState = connectionStateMap[connectionAsData];

                        if (connectionState.StartPointAsData == connnectionPointAsData)
                        {
                            connectionState.StartPoint = connectionPoint;
                        }
                        if (connectionState.EndPointAsData == connnectionPointAsData)
                        {
                            connectionState.EndPoint = connectionPoint;
                        }

                        if (connectionState.IsValid)
                        {
                            node.Connections.Add(connectionState.TheConnection);
                            connectionStateMap.Remove(connectionAsData);
                        }

                        selectedForRemoval.Add(connectionAsData);
                    }

                    foreach (object completedConnection in selectedForRemoval)
                    {
                        connectionList.Remove(completedConnection);
                    }

                    if (connectionList.Count == 0)
                    {
                        pointConnectionMap.Remove(connnectionPointAsData);
                    }
                }
            }
        }

        private SelectionService<GraphDesigner> SelectionService
        {
            get;
            set;
        }

        private Canvas GetConnectionPanel()
        {
            if(connectionPanel == null)
                connectionPanel = (Canvas)this.Template.FindName("PART_ConnectionArea", this);
            return connectionPanel;
        }

        private static T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);

            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        internal void AddConnectionAsData(object connectionAsData)
        {
            GraphDataTemplate graphDataTemplate = ItemTemplate as GraphDataTemplate;

            if (!connectionStateMap.ContainsKey(connectionAsData))
            {
                connectionStateMap.Add(connectionAsData, new ConnectionState());
            }

            object startPoint = graphDataTemplate.GetStartPoint(connectionAsData);
            connectionStateMap[connectionAsData].StartPointAsData = startPoint;

            object endPoint = graphDataTemplate.GetEndPoint(connectionAsData);
            connectionStateMap[connectionAsData].EndPointAsData = endPoint;

            if (!pointConnectionMap.ContainsKey(startPoint))
            {
                pointConnectionMap.Add(startPoint, new List<object>());
            }
            pointConnectionMap[startPoint].Add(connectionAsData);

            if (!pointConnectionMap.ContainsKey(endPoint))
            {
                pointConnectionMap.Add(endPoint, new List<object>());
            }
            pointConnectionMap[endPoint].Add(connectionAsData);
        }

        internal void AddConnection(IEnumerable connectionList)
        {
            foreach (object connectionAsObj in connectionList)
            {
                Connection connection = connectionAsObj as Connection;
                AddConnection(connection);
            }
        }

        internal void AddConnection(Connection connection)
        {
            if (connection.DataContext == null && (ItemTemplate is GraphDataTemplate))
            {
                GraphDataTemplate graphDataTemplate = ItemTemplate as GraphDataTemplate;
                object connectionAsData = Activator.CreateInstance(connectionType);

                graphDataTemplate.SetStartPoint(connectionAsData, connection.SourceConnectionPoint.DataContext);
                graphDataTemplate.SetEndPoint(connectionAsData, connection.TargetConnectionPoint.DataContext);

                reportConnectionAdded = false;
                graphDataTemplate.AddConnection(connection.SourceConnectionPoint.Node.DataContext, connectionAsData);
                reportConnectionAdded = true;
                connection.DataContext = connectionAsData;
            }

            GetConnectionPanel().Children.Add(connection);
        }

        internal void RemoveConnectionAsData(List<object> connectionAsDataList)
        {
            foreach (object connectionAsData in connectionAsDataList)
            {
                RemoveConnectionAsData(connectionAsDataList);
            }        
        }

        internal void RemoveConnectionAsData(object connectionAsData)
        {
            connectionStateMap.Remove(connectionAsData);
        }

        internal void RemoveConnection(IEnumerable connectionList)
        {
            foreach (object connectionAsObj in connectionList)
            {
                Connection connection = connectionAsObj as Connection;
                RemoveConnection(connection);
            }
        }

        internal void RemoveConnection(Connection connection)
        {
            GetConnectionPanel().Children.Remove(connection);
        }

        private void SetConnectionType(Type connectionType)
        {
            this.connectionType = connectionType;
        }

        private class ConnectionState
        {
            public ConnectionState()
            {
                TheConnection = new Connection();
            }

            public Connection TheConnection { get; private set; }
            public object StartPointAsData { get; set; }
            public ConnectionPoint StartPoint 
            {
                get { return TheConnection.SourceConnectionPoint; }
                set { TheConnection.SourceConnectionPoint = value; }
            }
            public object EndPointAsData { get; set; }
            public ConnectionPoint EndPoint
            {
                get { return TheConnection.TargetConnectionPoint; }
                set { TheConnection.TargetConnectionPoint = value; }
            }

            public bool IsValid
            {
                get
                {
                    return (StartPoint != null) && (EndPoint != null);
                }
            }
        }

        private bool reportConnectionAdded = true;

        private Dictionary<object, List<object>> pointConnectionMap = new Dictionary<object, List<object>>();
        private Dictionary<object, ConnectionState> connectionStateMap = new Dictionary<object, ConnectionState>();
        private Canvas connectionPanel = null;

        private Type connectionType;
    }
}
