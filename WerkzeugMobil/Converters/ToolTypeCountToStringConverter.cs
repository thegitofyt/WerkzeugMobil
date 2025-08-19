using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;

namespace WerkzeugMobil.Converters
{
    public class ToolTypeCountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is List<Tuple<string, int>> list && list.Count > 0)
            {
                return $"{list[0].Item1} ({list[0].Item2})";
            }

            return "Keine Kennzahl";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException(); // nur für Anzeige
        }
    }
}
