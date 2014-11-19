using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace HFK.GraphDesigner.WPF
{
    public class MultiDockPanel : DockPanel
    {
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.RegisterAttached("Index", typeof(int), typeof(MultiDockPanel),
            new FrameworkPropertyMetadata(0,
                                          new PropertyChangedCallback(MultiDockPanel.OnIndexChanged)));

        public static int GetIndex(UIElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (int)element.GetValue(IndexProperty);
        }

        public static void SetIndex(UIElement element, int value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(IndexProperty, value);
        }

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIElement reference = d as UIElement;
            if (reference != null)
            {
                MultiDockPanel parent = VisualTreeHelper.GetParent(reference) as MultiDockPanel;
                if (parent != null)
                {
                    parent.InvalidateArrange();
                }
            }
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            Dictionary<Dock, List<ConnectionDefinition>> dockingMap = new Dictionary<Dock, List<ConnectionDefinition>>();
            dockingMap.Add(Dock.Left, new List<ConnectionDefinition>());
            dockingMap.Add(Dock.Right, new List<ConnectionDefinition>());
            dockingMap.Add(Dock.Top, new List<ConnectionDefinition>());
            dockingMap.Add(Dock.Bottom, new List<ConnectionDefinition>());

            foreach (UIElement element in base.InternalChildren)
            {
                if (element != null)
                {
                    Dock dockingSide = GetDock(element);
                    dockingMap[dockingSide].Add(new ConnectionDefinition() { ConnectionPoint = element });
                }
            }

            if(dockingMap.ContainsKey(Dock.Left))
            {
                int numberOfLeftDockingElements = dockingMap[Dock.Left].Count;
                int index = 1;
                foreach (UIElement element in (dockingMap[Dock.Left].OrderBy(x => x.Index).Select(x => x.ConnectionPoint)))
                {
                    double y = (index * arrangeSize.Height / (numberOfLeftDockingElements + 1)) - (element.DesiredSize.Height / 2);

                    if (double.IsNaN(y)) y = 0;

                    element.Arrange(new Rect(new Point(0, y), element.DesiredSize));
                    index++;
                }
            }

            if (dockingMap.ContainsKey(Dock.Right))
            {
                int numberOfLeftDockingElements = dockingMap[Dock.Right].Count;
                int index = 1;
                foreach (UIElement element in (dockingMap[Dock.Right].OrderBy(x => x.Index).Select(x => x.ConnectionPoint)))
                {
                    double y = (index * arrangeSize.Height / (numberOfLeftDockingElements + 1)) - (element.DesiredSize.Height / 2);

                    if (double.IsNaN(y)) y = 0;

                    element.Arrange(new Rect(new Point(arrangeSize.Width - element.DesiredSize.Width, y), element.DesiredSize));
                    index++;
                }
            }

            if (dockingMap.ContainsKey(Dock.Top))
            {
                int numberOfLeftDockingElements = dockingMap[Dock.Top].Count;
                int index = 1;
                foreach (UIElement element in (dockingMap[Dock.Top].OrderBy(x => x.Index).Select(x => x.ConnectionPoint)))
                {
                    double x = (index * arrangeSize.Width / (numberOfLeftDockingElements + 1)) - (element.DesiredSize.Width / 2);

                    if (double.IsNaN(x)) x = 0;

                    element.Arrange(new Rect(new Point(x, 0), element.DesiredSize));
                    index++;
                }
            }

            if (dockingMap.ContainsKey(Dock.Bottom))
            {
                int numberOfLeftDockingElements = dockingMap[Dock.Bottom].Count;
                int index = 1;
                foreach (UIElement element in (dockingMap[Dock.Bottom].OrderBy(x => x.Index).Select(x => x.ConnectionPoint)))
                {
                    double x = (index * arrangeSize.Width / (numberOfLeftDockingElements + 1)) - (element.DesiredSize.Width / 2);

                    if (double.IsNaN(x)) x = 0;

                    element.Arrange(new Rect(new Point(x, arrangeSize.Height - element.DesiredSize.Height), element.DesiredSize));
                    index++;
                }
            }

            return arrangeSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size size = new Size(double.PositiveInfinity, double.PositiveInfinity);

            foreach (UIElement element in this.InternalChildren)
            {
                if (element != null)
                    element.Measure(size);
            }

            return base.MeasureOverride(availableSize);
        }

        private struct ConnectionDefinition
        {
            public UIElement ConnectionPoint { get; set; }
            public int Index { get; set; }
        }
    }
}
