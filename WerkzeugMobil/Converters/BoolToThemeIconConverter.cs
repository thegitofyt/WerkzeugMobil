using FontAwesome.WPF;
using System;
using System.Globalization;
using System.Windows.Data;

namespace WerkzeugMobil.Converters
{
    public class BoolToThemeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isDark = (bool)value;
            return isDark ? FontAwesomeIcon.SunOutline : FontAwesomeIcon.MoonOutline;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}