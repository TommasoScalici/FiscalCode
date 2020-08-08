using System;
using System.Globalization;

using Xamarin.Forms;

namespace FiscalCode.Converters
{
    public class BoolNegationConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value is bool boolean && boolean);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(value is bool boolean && boolean);
    }
}
