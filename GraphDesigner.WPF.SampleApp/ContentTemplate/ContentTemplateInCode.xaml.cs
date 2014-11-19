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
    /// Interaction logic for ContentTemplateInCode.xaml
    /// </summary>
    public partial class ContentTemplateInCode : UserControl
    {
        public ContentTemplateInCode()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<GraphNode> itemsSource = new ObservableCollection<GraphNode>();

            GraphNode node1 = new GraphNode() { PosX = 10, PosY = 10, Content = "Test 1", ContentTemplate = GetNodeTemplate() };

            itemsSource.Add(node1);
            ObservableCollection<ConnectionPoint> connectionPointsSource1 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connection11 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connection11.ContentTemplate = GetConnectionPointTemplate();
            connectionPointsSource1.Add(connection11);
            ConnectionPoint connection12 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource1.Add(connection12);
            itemsSource[0].ItemsSource = connectionPointsSource1;

            itemsSource.Add(new GraphNode() { PosX = 20, PosY = 50, Content = "Test 2", ContentTemplate = GetNodeTemplate() });
            ObservableCollection<ConnectionPoint> connectionPointsSource2 = new ObservableCollection<ConnectionPoint>();
            ConnectionPoint connection21 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection21);
            ConnectionPoint connection22 = new ConnectionPoint() { Name = "", Dock = System.Windows.Controls.Dock.Bottom, Index = 0 };
            connectionPointsSource2.Add(connection22);
            itemsSource[1].ItemsSource = connectionPointsSource2;

            node1.Connections.Add(new Connection() { SourceConnectionPoint = connection11, TargetConnectionPoint = connection21 });

            MyDiagramControl.ItemsSource = itemsSource;
        }

        private System.Windows.DataTemplate GetNodeTemplate()
        {
            //create the data template
            System.Windows.DataTemplate dataTemplate = new System.Windows.DataTemplate();

            //create stack pane;
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "parentStackpanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // Create check box
            FrameworkElementFactory checkBox = new FrameworkElementFactory(typeof(CheckBox));
            checkBox.Name = "chk";
            checkBox.SetValue(CheckBox.NameProperty, "chk");
            checkBox.SetValue(CheckBox.TagProperty, new Binding());
            checkBox.SetValue(CheckBox.MarginProperty, new Thickness(2));
            stackPanel.AppendChild(checkBox);

            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof(TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding());
            label.SetValue(TextBlock.ToolTipProperty, new Binding());

            stackPanel.AppendChild(label);


            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            return dataTemplate;

        }

        private System.Windows.DataTemplate GetConnectionPointTemplate()
        {
            //create the data template
            System.Windows.DataTemplate dataTemplate = new System.Windows.DataTemplate();

            //create stack pane;
            FrameworkElementFactory rectanglePanel = new FrameworkElementFactory(typeof(Rectangle));
            rectanglePanel.Name = "parentStackpanel";
            rectanglePanel.SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.Lavender));

            //set the visual tree of the data template
            dataTemplate.VisualTree = rectanglePanel;

            return dataTemplate;

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
    }
}
