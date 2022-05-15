using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Converters
{
    public class RoleIdToStringValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)value == 0)
            {
                return null;
            }
            return (int)value == 1 ? "Участник" : "Организатор";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == "Участник" ? 1 : 2;
        }
    }
}
