using System;
using System.Globalization;
using System.Windows.Data;

namespace WerkzeugMobil.Converters
{
    public class GeräteKennzahlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";

            // ToolDto mit ToolTypeCounts
            var toolTypeCountsProp = value.GetType().GetProperty("ToolTypeCounts");
            if (toolTypeCountsProp != null)
            {
                var list = toolTypeCountsProp.GetValue(value) as System.Collections.IList;
                if (list != null && list.Count > 0)
                    return list[0]?.ToString();
            }

            // WerkzeugDto mit WerkzeugId
            var werkzeugIdProp = value.GetType().GetProperty("WerkzeugId");
            if (werkzeugIdProp != null)
            {
                return werkzeugIdProp.GetValue(value)?.ToString();
            }

            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing; // One-way binding
        }
    }
}
