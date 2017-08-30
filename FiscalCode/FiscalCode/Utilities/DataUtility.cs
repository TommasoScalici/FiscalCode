using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FiscalCode.Utilities
{
    public class DataUtility
    {
        public static Stream LoadStreamFromResource(string resourceName)
        {
            var assembly = typeof(DataUtility).GetTypeInfo().Assembly;
            var resourceNames = assembly.GetManifestResourceNames();

            if (resourceNames.Any(x => x.Contains(resourceName)))
            {
                var name = string.Empty;

                try
                {
                    name = resourceNames.Single(x => x.Contains(resourceName));
                }
                catch (InvalidOperationException)
                {
                    var lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
                    name = resourceNames.FirstOrDefault(x => x.Contains(resourceName) && x.Contains(lang));
                }

                if (string.IsNullOrWhiteSpace(name))
                    return Stream.Null;

                return assembly.GetManifestResourceStream(name);
            }

            return Stream.Null;
        }
    }
}
