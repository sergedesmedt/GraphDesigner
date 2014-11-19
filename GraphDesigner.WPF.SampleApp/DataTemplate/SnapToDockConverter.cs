using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace HFK.GraphDesigner.WPF.SampleApp.DataTemplate
{
    public class SnapToDockConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (targetType == typeof(Dock) && value.GetType() == typeof(string))
            {
                if ((value as string) == "Up")
                {
                    return Dock.Top;
                }
                if ((value as string) == "Down")
                {
                    return Dock.Bottom;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
