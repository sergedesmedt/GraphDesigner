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
    /// Interaction logic for CustomGraphCustomClass.xaml
    /// </summary>
    public partial class CustomGraphCustomClass : UserControl
    {
        public CustomGraphCustomClass()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<GraphNode> itemsSource = new ObservableCollection<GraphNode>();

            CustomGraphNode node1 = GetGraphNode(10, 10, "Test 1", "channel_merger.png");
            itemsSource.Add(node1);
            ObservableCollection<ConnectionPoint> connectionPointsSource1 = new ObservableCollection<ConnectionPoint>();
            CustomConnectionPoint connection11 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connection11);
            CustomConnectionPoint connection12 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connection12);
            itemsSource[0].ItemsSource = connectionPointsSource1;

            itemsSource.Add(GetGraphNode(20, 70, "Test 2", "channel_splitter.png"));
            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            CustomConnectionPoint connection21 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection21);
            CustomConnectionPoint connection22 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection22);
            itemsSource[1].ItemsSource = connectionPointsSource2;

            node1.Connections.Add(new Connection() { SourceConnectionPoint = connection11, TargetConnectionPoint = connection21 });

            MyDiagramControl.ItemsSource = itemsSource;
        }

        private CustomGraphNode GetGraphNode(int posX, int posY, string text, string imageUrl)
        {
            CustomGraphNode item = new CustomGraphNode();
            item.PosX = posX;
            item.PosY = posY;

            item.Text = text;

            if (imageUrl.Length > 0)
                item.ImageUrl = new Uri("pack://application:,,/Images/" + imageUrl);


            return item;

        }

        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            GraphNode node = GetGraphNode(50, 50, "Test", "channel_merger.png");

            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connectionPoint21 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connectionPoint21);
            ConnectionPoint connectionPoint22 = new CustomConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
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
