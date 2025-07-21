using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil.Converters
{
    public class ProjektWerkzeugCountConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return "";

            var projekt = values[0] as ProjektDTO;
            var viewModel = values[1] as ProjekteViewModel;

            if (projekt == null || viewModel == null)
                return "";

            if (viewModel.WerkzeugCounts.TryGetValue(projekt.ProjektAddresse, out int count))
            {
                return $"{count} Werkzeuge";
            }

            return "0 Werkzeuge";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}