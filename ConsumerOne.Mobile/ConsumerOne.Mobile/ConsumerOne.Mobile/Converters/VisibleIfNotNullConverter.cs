using System;
using System.Globalization;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.Converters
{
    public class VisibleIfNotNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter ==null? value != null : value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
