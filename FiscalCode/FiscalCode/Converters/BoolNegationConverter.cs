using System;
using System.Globalization;

using Xamarin.Forms;

namespace FiscalCode.Converters
{
    public class BoolNegationConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => !(value is bool && (bool)value);
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => !(value is bool && (bool)value);
    }
}
