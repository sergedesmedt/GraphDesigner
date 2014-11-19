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

namespace HFK.GraphDesigner.WPF.SampleApp.Custom
{
    /// <summary>
    /// Interaction logic for CustomGraphInCode.xaml
    /// </summary>
    public partial class CustomGraphInCode : UserControl
    {
        public CustomGraphInCode()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<GraphNode> itemsSource = new ObservableCollection<GraphNode>();

            GraphNode node1 = GetGraphNode(10, 10, "Test 1", Colors.Green);
            itemsSource.Add(node1);
            ObservableCollection<ConnectionPoint> connectionPointsSource1 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connection11 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connection11);
            ConnectionPoint connection12 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connection12);
            itemsSource[0].ItemsSource = connectionPointsSource1;

            itemsSource.Add(GetGraphNode(20, 70, "Test 2", Colors.Red));
            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connection21 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection21);
            ConnectionPoint connection22 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection22);
            itemsSource[1].ItemsSource = connectionPointsSource2;

            node1.Connections.Add(new Connection() { SourceConnectionPoint = connection11, TargetConnectionPoint = connection21 });

            MyDiagramControl.ItemsSource = itemsSource;
        }

        private GraphNode GetGraphNode(int posX, int posY, string text, Color boxColor)
        {
            GraphNode item = new GraphNode();
            item.PosX = posX;
            item.PosY = posY;

            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            Border border = new Border();
            border.Width = 8;
            border.Height = 12;
            border.Background = new SolidColorBrush(boxColor);

            Label lbl = new Label();
            lbl.Content = text;

            stack.Children.Add(border);
            stack.Children.Add(lbl);

            item.Content = stack;
            return item;

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
            Connection connection = new Connection()
            {
                SourceConnectionPoint = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Items[0] as ConnectionPoint,
                TargetConnectionPoint = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 1] as GraphNode).Items[0] as ConnectionPoint
            };
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Connections.Add(connection);

        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.ItemsSource as ObservableCollection<GraphNode>).RemoveAt(0);
        }

        private void btnRemoveConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Connections.RemoveAt(0);
        }
    }
}
