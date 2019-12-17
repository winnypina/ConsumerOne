using System;
using System.Globalization;
using System.Linq;
using ConsumerOne.Mobile.ViewModels.Models;
using MvvmCross.ViewModels;
using Xamarin.Forms;

namespace ConsumerOne.Mobile.Converters
{
    public class TranslatedTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var texts = (MvxObservableCollection<TranslationModel>) value;
            return texts.SingleOrDefault(n => n.Key == parameter.ToString())?.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}