using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace HFK.GraphDesigner.WPF
{
    public class MoveThumb : Thumb
    {
        private GraphNode m_item;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.m_item = DataContext as GraphNode;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (m_item != null)
            {
                m_item.PosX = m_item.PosX + e.HorizontalChange;
                m_item.PosY = m_item.PosY + e.VerticalChange;

                e.Handled = true;
            }
        }
    }
}
