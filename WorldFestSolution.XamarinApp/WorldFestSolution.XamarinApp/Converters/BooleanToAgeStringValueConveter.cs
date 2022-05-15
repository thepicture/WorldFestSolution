using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Converters
{
    public class BooleanToAgeStringValueConveter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((value as bool?).HasValue)
            {
                return (value as bool?).Value ? "Мне уже есть 18" : "Мне меньше 18";
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue == "Мне уже есть 18";
            }
            else
            {
                return false;
            }
        }
    }
}
