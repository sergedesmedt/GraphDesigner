using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace HFK.GraphDesigner.WPF
{
    public class GraphDataTemplate : DataTemplate
    {
        public BindingBase ItemsSource
        {
            get;
            set;
        }

        public DataTemplate ItemTemplate
        {
            get;
            set;
        }

        public string XPos
        {
            get;
            set;
        }

        public string YPos
        {
            get;
            set;
        }

        public string Docking
        {
            get;
            set;
        }

        public IValueConverter DockingConverter
        {
            get;
            set;
        }

        public string ConnectionsSource
        {
            get;
            set;
        }

        public string ConnectionsStartPoint
        {
            get;
            set;
        }

        public string ConnectionsEndPoint
        {
            get;
            set;
        }

        public virtual object GetDocking(object connectionPointAsData)
        {
            PropertyInfo dockingProperty = connectionPointAsData.GetType().GetProperty(Docking);
            object docking = dockingProperty.GetValue(connectionPointAsData, null);
            return docking;
        }

        public virtual object GetStartPoint(object connectionAsData)
        {
            PropertyInfo startPointProperty = connectionAsData.GetType().GetProperty(ConnectionsStartPoint);
            object startPoint = startPointProperty.GetValue(connectionAsData, null);
            return startPoint;
        }

        public virtual void SetStartPoint(object connectionAsData, object connectionPointAsData)
        {
            PropertyInfo startPointProperty = connectionAsData.GetType().GetProperty(ConnectionsStartPoint);
            startPointProperty.SetValue(connectionAsData, connectionPointAsData, null);
        }

        public virtual object GetEndPoint(object connectionAsData)
        {
            PropertyInfo startPointProperty = connectionAsData.GetType().GetProperty(ConnectionsEndPoint);
            object startPoint = startPointProperty.GetValue(connectionAsData, null);
            return startPoint;
        }

        public virtual void SetEndPoint(object connectionAsData, object connectionPointAsData)
        {
            PropertyInfo endPointProperty = connectionAsData.GetType().GetProperty(ConnectionsEndPoint);
            endPointProperty.SetValue(connectionAsData, connectionPointAsData, null);
        }

        public virtual IEnumerable GetConnections(object nodeAsData)
        {
            PropertyInfo connectionListProperty = nodeAsData.GetType().GetProperty(ConnectionsSource);
            object connectionListAsObject = connectionListProperty.GetValue(nodeAsData, null);
            return (connectionListAsObject as IEnumerable);
        }

        public virtual Type GetConnectionsType(object nodeAsData)
        {
            PropertyInfo connectionListProperty = nodeAsData.GetType().GetProperty(ConnectionsSource);
            Type connectionListType = connectionListProperty.PropertyType;
            foreach (Type arg in connectionListType.GetGenericArguments())
            {
                return arg;
            }

            throw new Exception();
        }

        public virtual void AddConnection(object nodeAsData, object connectionAsData)
        {
            IEnumerable connections = GetConnections(nodeAsData);

            if (!(connections is IList))
            {
            }

            (connections as IList).Add(connectionAsData);
        }

    }
}
