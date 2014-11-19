using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace HFK.GraphDesigner.WPF.SampleApp.Simple
{
    /// <summary>
    /// Interaction logic for SimpleGraphInCode.xaml
    /// </summary>
    public partial class SimpleGraphInCode : UserControl
    {
        public SimpleGraphInCode()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<GraphNode> itemsSource = new ObservableCollection<GraphNode>();

            GraphNode node2 = new GraphNode() { PosX = 20, PosY = 50, Content = "Test 2" };
            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connectionPoint21 = new ConnectionPoint() { Name = "", SomeName = "pt21", Dock = System.Windows.Controls.Dock.Top, Index = 0 };
            connectionPointsSource2.Add(connectionPoint21);

            GraphNode node1 = new GraphNode() { PosX = 100, PosY = 100, Content = "Test 1" };
            ObservableCollection<ConnectionPoint> connectionPointsSource1 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connectionPoint11 = new ConnectionPoint() { Name = "", SomeName="pt11", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connectionPoint11);
            
            node1.ItemsSource = connectionPointsSource1;
            node2.ItemsSource = connectionPointsSource2;

            itemsSource.Add(node1);
            itemsSource.Add(node2);

            node2.Connections.Add(new Connection() { SourceConnectionPoint = connectionPoint21, TargetConnectionPoint = connectionPoint11 });

            MyDiagramControl.ItemsSource = itemsSource;
        }

        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            GraphNode node = new GraphNode();
            node.PosX = 50;
            node.PosY = 50;
            node.Content = "Test";

            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connectionPoint21 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connectionPoint21);
            ConnectionPoint connectionPoint22 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connectionPoint22);
            node.ItemsSource = connectionPointsSource2;

            (MyDiagramControl.ItemsSource as ObservableCollection<GraphNode>).Add(node);
        }

        private void btnAddConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Connections.Add
            (
                new Connection()
                {
                    SourceConnectionPoint = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Items[0] as ConnectionPoint,
                    TargetConnectionPoint = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 1] as GraphNode).Items[0] as ConnectionPoint
                }
            );

        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.ItemsSource as ObservableCollection<GraphNode>).RemoveAt(0);
        }

        private void btnRemoveConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Connections.RemoveAt(0);
        }

        private void btnUninsertedNode_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddIncompleteConnection_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
