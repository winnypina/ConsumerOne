using System;
using System.Globalization;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.Converters
{
    public class HigherThenVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var oriValue = System.Convert.ToInt32(value);
            var parValue = System.Convert.ToInt32(parameter);
            return oriValue >= parValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
