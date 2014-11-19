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

namespace HFK.GraphDesigner.WPF.SampleApp.DataTemplate
{
    /// <summary>
    /// Interaction logic for DataTemplateInCode.xaml
    /// </summary>
    public partial class DataTemplateInCode : UserControl
    {
        public DataTemplateInCode()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<CustomNodeDataClass> itemsSource = new ObservableCollection<CustomNodeDataClass>();

            CustomNodeDataClass node1 = GetCustomClass(10, 10, "Test 1", "channel_merger.png");
            itemsSource.Add(node1);
            ObservableCollection<CustomConnectionPointDataClass> connectionPointsSource1 = new ObservableCollection<CustomConnectionPointDataClass>();
            CustomConnectionPointDataClass connection11 = new CustomConnectionPointDataClass() { Number = "11" };
            connectionPointsSource1.Add(connection11);
            CustomConnectionPointDataClass connection12 = new CustomConnectionPointDataClass() { Number = "12" };
            connectionPointsSource1.Add(connection12);
            itemsSource[0].PointList = connectionPointsSource1;

            itemsSource.Add(GetCustomClass(50, 70, "Test 2", "channel_splitter.png"));
            ObservableCollection<CustomConnectionPointDataClass> connectionPointsSource2 = new ObservableCollection<CustomConnectionPointDataClass>();
            CustomConnectionPointDataClass connection21 = new CustomConnectionPointDataClass() { Number = "21" };
            connectionPointsSource2.Add(connection21);
            CustomConnectionPointDataClass connection22 = new CustomConnectionPointDataClass() { Number = "22" };
            connectionPointsSource2.Add(connection22);
            itemsSource[1].PointList = connectionPointsSource2;

            node1.ConnectionList = new ObservableCollection<CustomConnection>();
            node1.ConnectionList.Add(new CustomConnection() { Start = connection11, End = connection21 });

            MyDiagramControl.ItemTemplate = GetNodeTemplate();

            MyDiagramControl.ItemsSource = itemsSource;

        }

        private CustomNodeDataClass GetCustomClass(int posX, int posY, string text, string imageUrl)
        {
            CustomNodeDataClass item = new CustomNodeDataClass();
            item.XCoord = posX;
            item.YCoord = posY;

            item.Name = text;

            if (imageUrl.Length > 0)
                item.ImageUrl = new Uri("pack://application:,,/Images/" + imageUrl);


            return item;
        }

        private System.Windows.DataTemplate GetNodeTemplate()
        {
            //create the data template
            GraphDataTemplate dataTemplate = new GraphDataTemplate();
            dataTemplate.XPos = "XCoord";
            dataTemplate.YPos = "YCoord";
            dataTemplate.ConnectionsSource = "ConnectionList";
            dataTemplate.ConnectionsStartPoint = "Start";
            dataTemplate.ConnectionsEndPoint = "End";
            dataTemplate.ItemsSource = new Binding("PointList");

            //create stack pane;
            FrameworkElementFactory stackPanel = new FrameworkElementFactory(typeof(StackPanel));
            stackPanel.Name = "parentStackpanel";
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            // create text
            FrameworkElementFactory label = new FrameworkElementFactory(typeof(TextBlock));
            label.SetBinding(TextBlock.TextProperty, new Binding() { Path = new PropertyPath("Name") });

            stackPanel.AppendChild(label);

            //set the visual tree of the data template
            dataTemplate.VisualTree = stackPanel;

            System.Windows.DataTemplate itemTemplate = new System.Windows.DataTemplate();
            dataTemplate.ItemTemplate = itemTemplate;

            FrameworkElementFactory rectangle = new FrameworkElementFactory(typeof(Rectangle));
            //rectangle.Name = "parentStackpanel";
            rectangle.SetValue(Rectangle.FillProperty, new SolidColorBrush(Colors.Green));

            itemTemplate.VisualTree = rectangle;

            return dataTemplate;

        }
        private void btnAddNode_Click(object sender, RoutedEventArgs e)
        {
            CustomNodeDataClass node = GetCustomClass(50, 50, "Test", "channel_merger.png");

            ObservableCollection<CustomConnectionPointDataClass> connectionPointsSource1 = new ObservableCollection<CustomConnectionPointDataClass>();
            CustomConnectionPointDataClass connection11 = new CustomConnectionPointDataClass() { Number = "01" };
            connectionPointsSource1.Add(connection11);
            CustomConnectionPointDataClass connection12 = new CustomConnectionPointDataClass() { Number = "02" };
            connectionPointsSource1.Add(connection12);
            node.PointList = connectionPointsSource1;

            (MyDiagramControl.ItemsSource as ObservableCollection<CustomNodeDataClass>).Add(node);
        }

        private void btnAddConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as CustomNodeDataClass).ConnectionList.Add
            (
                new CustomConnection()
                {
                    Start = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as CustomNodeDataClass).PointList[0] as CustomConnectionPointDataClass,
                    End = (MyDiagramControl.Items[MyDiagramControl.Items.Count - 1] as CustomNodeDataClass).PointList[0] as CustomConnectionPointDataClass
                }
            );
        }

        private void btnRemoveNode_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.ItemsSource as ObservableCollection<CustomNodeDataClass>).RemoveAt(0);
        }

        private void btnRemoveConn_Click(object sender, RoutedEventArgs e)
        {
            (MyDiagramControl.Items[MyDiagramControl.Items.Count - 2] as CustomNodeDataClass).ConnectionList.RemoveAt(0);
        }
    }
}
