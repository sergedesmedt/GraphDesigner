using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Documents;

namespace HFK.GraphDesigner.WPF
{
    public class ConnectionPoint : ContentControl, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        static ConnectionPoint()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectionPoint), new FrameworkPropertyMetadata(typeof(ConnectionPoint)));
        }

        public ConnectionPoint()
        {
            this.LayoutUpdated += new EventHandler(ConnectionPoint_LayoutUpdated);
        }

        bool updatingLayout = false;
        void ConnectionPoint_LayoutUpdated(object sender, EventArgs e)
        {
            if (updatingLayout)
            {
                return;
            }

            try
            {
                updatingLayout = true;
                if (Diagram != null)
                {
                    if (!Diagram.IsAncestorOf(this))
                    {
                        this.ConnectAt = new Point(int.MaxValue, int.MaxValue);
                        return;
                    }
                    this.ConnectAt = this.TransformToAncestor(Diagram).Transform(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
                }

                updatingLayout = false;
            }
            catch (Exception ex)
            {
                updatingLayout = false;
                throw;
            }
        }

        public string SomeName
        {
            get;
            set;
        }

        public Dock Dock
        {
            get 
            { 
                return DockPanel.GetDock(this); 
            }
            set 
            {
                DockPanel.SetDock(this, value); 
            }
        }

        public int Index
        {
            get { return MultiDockPanel.GetIndex(this); }
            set { MultiDockPanel.SetIndex(this, value); }
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

        public GraphNode Node
        {
            get
            {
                if (node == null)
                {
                    DependencyObject element = this;
                    while (element != null && !(element is GraphNode))
                        element = VisualTreeHelper.GetParent(element);
                    node = element as GraphNode;
                }

                return node;
            }
        }

        public Point ConnectAt
        {
            get
            {
                return m_connectAt;
            }
            private set
            {
                m_connectAt = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ConnectAt"));
                }
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            if (Diagram != null)
            {
                m_mouseDownPoint = e.GetPosition(Diagram);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed) 
            {
                this.m_mouseDownPoint = null;
            }

            if (this.m_mouseDownPoint.HasValue)
            {
                if (isDragging)
                {
                    adorner.EndPoint = e.GetPosition(Diagram);

                    DependencyObject hitObject = Diagram.InputHitTest(e.GetPosition(Diagram)) as DependencyObject;
                    bool targetConnectionPointIsSet = false;
                    while (hitObject != null &&
                           hitObject.GetType() != typeof(GraphDesigner))
                    {
                        if (hitObject is ConnectionPoint && hitObject != this)
                        {
                            ConnectionPoint connectionPoint = hitObject as ConnectionPoint;
                            if (targetConnectionPoint != null && targetConnectionPoint == connectionPoint)
                            {
                                targetConnectionPointIsSet = true;
                            }
                            if (targetConnectionPoint == null)
                            {
                                targetConnectionPoint = connectionPoint;
                                targetConnectionPointIsSet = true;
                            }
                        }
                        if (hitObject is GraphNode && hitObject != this.Node)
                        {
                            if (!targetConnectionPointIsSet && targetConnectionPoint != null)
                            {
                                targetConnectionPoint = null;
                            }
                            return;
                        }
                        hitObject = VisualTreeHelper.GetParent(hitObject);
                    }
                }
                else
                {
                    if (Diagram != null)
                    {
                        AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Diagram);
                        if (adornerLayer != null)
                        {
                            adorner = new ConnectionAdorner(Diagram);
                            adorner.StartPoint = m_mouseDownPoint.Value;
                            adorner.EndPoint = adorner.StartPoint;
                            adornerLayer.Add(adorner);
                            isDragging = true;
                            this.CaptureMouse();
                            e.Handled = true;
                        }
                    }
                }
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);

            this.ReleaseMouseCapture();

            if (isDragging && Diagram != null)
            {
                if (targetConnectionPoint != null)
                {
                    Connection connection = new Connection();
                    connection.SourceConnectionPoint = this;
                    connection.TargetConnectionPoint = targetConnectionPoint;
                    Node.Connections.Add(connection);

                    targetConnectionPoint = null;
                }
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Diagram);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(adorner);
                    isDragging = false;
                }
            }
        }

        Point? m_mouseDownPoint;
        Point m_connectAt = new Point(int.MaxValue, int.MaxValue);
        GraphDesigner graph;
        GraphNode node;
        ConnectionAdorner adorner;
        ConnectionPoint targetConnectionPoint;
        bool isDragging = false;
    }

}
