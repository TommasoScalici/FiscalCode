using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        public string Text { get; set; }


        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return null;

            Debug.WriteLine($"Provide: {Text}; Translated: {Locale.Localize(Text)}");
            return Locale.Localize(Text);
        }
    }
}
