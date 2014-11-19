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
using HFK.GraphDesigner.WPF;
using System.Collections.ObjectModel;

namespace HFK.GraphDesigner.WPF.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ShowControl(Control control, string title)
        {

            control.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            control.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            control.Width = double.NaN;
            control.Height = double.NaN;

            controlContainer.Children.Clear();
            controlContainer.Children.Add(control);
        }

        //Create Simple Graph
        // Simple content of type string
        private void btnSimpleXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new Simple.SimpleGraphUsingXaml(), "Simple Graph using xaml");
        }

        //Create Simple Graph
        // Simple content of type string
        private void btnSimpleInCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new Simple.SimpleGraphInCode(), "Simple Graph in code");
        }

        //Customize Graph
        // Specify content for content property of graphnode and connectionpoint
        private void btnCustomByXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new Custom.CustomGraphUsingXaml(), "Custom Graph using xaml");
        }

        //Customize Graph
        // Specify content for content property of graphnode and connectionpoint
        private void btnCustomByCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new Custom.CustomGraphInCode(), "Custom Graph in code");
        }

        //Customize Graph
        // Specify content for content property of graphnode and connectionpoint
        private void btnCustomByInheritance_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new Custom.CustomGraphCustomClass(), "Custom Graph by custom class");
        }

        //Content Template
        // using Style on graphnode and connectionpoint type specifying contenttemplate
        private void btnContentTemplateByXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ContentTemplate.ContentTemplateUsingXaml(), "Node template Graph using xaml");
        }

        //Content Template
        // using Style on graphnode and connectionpoint type specifying contenttemplate
        private void btnContentTemplateByCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new ContentTemplate.ContentTemplateInCode(), "Node template Graph using code");
        }

        //DataTemplate
        // using GraphDataTemplate for describing dataobjects
        private void btnDataTemplateByXaml_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new DataTemplate.DataTemplateUsingXaml(), "Item template Graph using xaml");
        }

        ////DataTemplate
        //// puting datatemplates in the resources section
        //private void btnDataTemplateByType_Click(object sender, RoutedEventArgs e)
        //{
        //    ShowControl(new DataTemplate.DataTemplateByType(), "Item template Graph by type");
        //}

        //DataTemplate
        // using GraphDataTemplate for describing dataobjects
        private void btnDataTemplateByCode_Click(object sender, RoutedEventArgs e)
        {
            ShowControl(new DataTemplate.DataTemplateInCode(), "Item template Graph using code");
        }
    }
}
