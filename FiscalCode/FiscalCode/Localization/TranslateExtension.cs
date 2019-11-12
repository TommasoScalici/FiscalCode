using Plugin.Multilingual;

using System;
using System.Reflection;
using System.Resources;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Localization
{
    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        const string resourceId = "FiscalCode.Resources.AppResources";

        static readonly Lazy<ResourceManager> resourceManager =
            new Lazy<ResourceManager>(() => new ResourceManager(resourceId, typeof(TranslateExtension).GetTypeInfo().Assembly));


        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            var currentCultureInfo = CrossMultilingual.Current.CurrentCultureInfo;
            var translation = resourceManager.Value.GetString(Text, currentCultureInfo);

            if (translation == null)
                translation = Text;

            return translation;
        }
    }
}
