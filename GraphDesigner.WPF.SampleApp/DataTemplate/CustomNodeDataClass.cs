using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace HFK.GraphDesigner.WPF.SampleApp.DataTemplate
{
    public class CustomNodeDataClass
    {
        public CustomNodeDataClass()
        {
            ConnectionList = new ObservableCollection<CustomConnection>();
        }

        #region Properties

        public int XCoord
        {
            get;
            set;
        }

        public int YCoord
        {
            get;
            set;
        }

        public Uri ImageUrl
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public ObservableCollection<CustomConnectionPointDataClass> PointList
        {
            get;
            set;
        }

        public ObservableCollection<CustomConnection> ConnectionList
        {
            get;
            set;
        }

        #endregion
    }
}
