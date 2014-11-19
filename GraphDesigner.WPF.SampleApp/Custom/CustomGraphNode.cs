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

namespace HFK.GraphDesigner.WPF.SampleApp.Custom
{
    public class CustomGraphNode : GraphNode
    {

        #region Data Member

        Uri _imageUrl = null;


        Image _image = null;
        TextBlock _textBlock = null;

        #endregion

        #region Properties

        public Uri ImageUrl
        {
            get { return _imageUrl; }
            set
            {
                _imageUrl = value;

                _image.Source = new BitmapImage(value);
            }
        }

        public string Text
        {
            get { return _textBlock.Text; }
            set { _textBlock.Text = value; }
        }

        #endregion

        #region Constructor

        public CustomGraphNode()
        {
            CreateGraphNodeContent();
        }

        #endregion

        #region Private Methods

        private void CreateGraphNodeContent()
        {
            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Horizontal;

            _image = new Image();
            _image.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            _image.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            _image.Width = 16;
            _image.Height = 16;
            _image.Margin = new Thickness(2);

            stack.Children.Add(_image);

            _textBlock = new TextBlock();
            _textBlock.Margin = new Thickness(2);
            _textBlock.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            stack.Children.Add(_textBlock);


            Content = stack;
        }

        #endregion

    }
}
