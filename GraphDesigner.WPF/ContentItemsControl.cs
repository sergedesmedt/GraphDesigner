using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace HFK.GraphDesigner.WPF
{
    public class ContentItemsControl : ItemsControl
    {

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ContentItemsControl));

        public object Content
        {
            get
            {
                return (double)GetValue(ContentProperty);
            }
            set
            {
                SetValue(ContentProperty, value);
            }
        }

        public static readonly DependencyProperty ContentTemplateProperty =
            DependencyProperty.Register("ContentTemplate", typeof(object), typeof(ContentItemsControl));

        public object ContentTemplate
        {
            get
            {
                return (object)GetValue(ContentTemplateProperty);
            }
            set
            {
                SetValue(ContentTemplateProperty, value);
            }
        }
    }
}
