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

namespace HFK.GraphDesigner.WPF.SampleApp.ContentTemplate
{
    /// <summary>
    /// Interaction logic for ContentTemplateUsingXaml.xaml
    /// </summary>
    public partial class ContentTemplateUsingXaml : UserControl
    {
        public ContentTemplateUsingXaml()
        {
            InitializeComponent();
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

            MyDiagramControl.Items.Add(node);
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
            MyDiagramControl.Items.RemoveAt(0);
        }

        private void btnRemoveConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as GraphNode).Connections.RemoveAt(0);
        }
    }
}
