using System;
using System.Globalization;
using System.Windows.Data;
using Cyclops.Xmpp.Data;

namespace Cyclops.MainApplication.ValueConverters
{
    public class StatusTypeToIntegerConverter : IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int) (StatusType)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (StatusType) (int) value;
        }

        #endregion
    }
}