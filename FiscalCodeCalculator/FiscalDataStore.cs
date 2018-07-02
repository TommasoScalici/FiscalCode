using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FiscalCodeCalculator
{
    public static class FiscalDataStore
    {
        static readonly List<District> districts = new List<District>();
        static readonly List<Nation> nations = new List<Nation>();


        static FiscalDataStore()
        {
            var districtsStream = LoadStreamFromResource("DistrictsList");
            var nationsStream = LoadStreamFromResource("NationsList");

            using (var reader = new StreamReader(districtsStream))
            {
                while (!reader.EndOfStream)
                {
                    var lineSplitted = reader.ReadLine().Split(';');
                    var district = new District(lineSplitted[0], lineSplitted[1], lineSplitted[2]);
                    districts.Add(district);
                }
            }

            using (var reader = new StreamReader(nationsStream))
            {
                while (!reader.EndOfStream)
                {
                    var lineSplitted = reader.ReadLine().Split(';');
                    var nation = new Nation(lineSplitted[0], lineSplitted[1]);
                    nations.Add(nation);
                }
            }
        }


        public static IEnumerable<District> Districts => districts;
        public static IEnumerable<Nation> Nations => nations;


        static Stream LoadStreamFromResource(string resourceName)
        {
            var assembly = typeof(FiscalDataStore).GetTypeInfo().Assembly;
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
