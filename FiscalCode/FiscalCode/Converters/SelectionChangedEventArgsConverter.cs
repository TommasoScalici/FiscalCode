using System;
using System.Globalization;

using Syncfusion.SfAutoComplete.XForms;
using Xamarin.Forms;

namespace FiscalCode.Converters
{
    public class SelectionChangedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is SelectionChangedEventArgs eventArgs))
                throw new ArgumentException($"Expected {nameof(SelectionChangedEventArgs)} as value", nameof(value));

            return eventArgs.Value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
               throw new NotImplementedException();
    }
}
