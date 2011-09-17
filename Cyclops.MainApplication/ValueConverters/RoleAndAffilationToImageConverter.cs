using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using Cyclops.Core;

namespace Cyclops.MainApplication.ValueConverters
{
    public class RoleAndAffilationToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Role))
                return null;
            Role role = (Role)value;
            const string uriTemplate = @"pack://application:,,,/Cyclops.MainApplication;component/Resources/{0}";
            switch (role)
            {
                case Role.Regular:
                    return string.Format(uriTemplate, "");
                case Role.Devoiced:
                    return string.Format(uriTemplate, "icon_shutup.png");
                case Role.Member:
                    return string.Format(uriTemplate, "medal_silver_2.png");
                case Role.Moder:
                    return string.Format(uriTemplate, "Moderator_icon.png");
                case Role.Admin:
                    return string.Format(uriTemplate, "Moderator_icon.png");
                case Role.Owner:
                    return string.Format(uriTemplate, "icon_crown.gif");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
