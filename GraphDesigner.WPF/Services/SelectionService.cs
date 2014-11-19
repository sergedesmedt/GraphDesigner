using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using HFK.GraphDesigner.WPF;

namespace HFK.GraphDesigner.WPF.Services
{
    public class SelectionService<T> where T : UIElement
    {
        public class SelectionChangedEventArgs : EventArgs
        {
            public SelectionChangedEventArgs(Control previousSelection, Control newSelection)
            {
                PreviousSelection = previousSelection;
                NewSelection = newSelection;
            }

            public Control PreviousSelection { get; private set; }
            public Control NewSelection { get; private set; }
        }

        public event EventHandler<SelectionChangedEventArgs> SelectionChangedHandler;

        public SelectionService(T designer)
        {
            Designer = designer;
        }

        public T Designer
        {
            private set
            {
                m_parent = value;
                m_parent.PreviewMouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(m_designer_PreviewMouseLeftButtonDown);
            }

            get
            {
                return m_parent;
            }

        }

        public Control SelectedObject
        {
            private set
            {
                Control previousSelectedObject = m_selectedObject;
                m_selectedObject = value;
                if (SelectionChangedHandler != null)
                {
                    SelectionChangedHandler(this,
                        new SelectionChangedEventArgs(previousSelectedObject, m_selectedObject));
                }
            }
            get
            {
                return m_selectedObject;
            }
        }

        void m_designer_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DependencyObject element = e.OriginalSource as DependencyObject;
            while (element != null)
            {
                if (element is ConnectionPoint)
                {
                    SelectedObject = e.Source as Control;
                    return;
                }

                if (element is GraphNode)
                {
                    SelectedObject = e.Source as Control;
                    return;
                }
                element = VisualTreeHelper.GetParent(element);
            }
        }

        private T m_parent;
        private Control m_selectedObject;
    }

}
