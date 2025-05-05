using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WerkzeugMobil
{
    public static class ThemeManager
    {
        public static void SetLightTheme(Application app)
        {
            app.Resources["AppBackground"] = app.Resources["LightBackground"];
            app.Resources["AppText"] = app.Resources["LightText"];
            app.Resources["AppCard"] = app.Resources["CardLight"];
            app.Resources["AppBorder"] = app.Resources["BorderLight"];
        }

        public static void SetDarkTheme(Application app)
        {
            app.Resources["AppBackground"] = app.Resources["DarkBackground"];
            app.Resources["AppText"] = app.Resources["DarkText"];
            app.Resources["AppCard"] = app.Resources["CardDark"];
            app.Resources["AppBorder"] = app.Resources["BorderDark"];
        }
    }
}