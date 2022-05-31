using System;
using System.Globalization;
using Xamarin.Forms;

namespace WorldFestSolution.XamarinApp.Converters
{
    public class StringToHasErrorIfEmptyValueConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            if (!(value is string valueAsString))
            {
                return true;
            }
            return string.IsNullOrWhiteSpace(valueAsString);
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
