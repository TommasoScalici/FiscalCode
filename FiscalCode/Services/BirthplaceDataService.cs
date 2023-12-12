using System.Globalization;
using System.Text.Json;

using FiscalCode.Data;

namespace FiscalCode.Services;
public class BirthplaceDataService
{
    private readonly List<BirthplaceDTO> birthplaces = [];


    public async Task<IEnumerable<BirthplaceDTO>> GetBirthplacesAsync()
    {
        if (birthplaces.Count == 0)
        {
            var currentCulture = CultureInfo.CurrentCulture;

            if (currentCulture is not null)
            {
                using var citiesStream = await FileSystem.OpenAppPackageFileAsync("cities.json");
                var cities = await JsonSerializer.DeserializeAsync<IEnumerable<BirthplaceDTO>>(citiesStream);

                var lang = currentCulture.TwoLetterISOLanguageName;
                using var statesStream = await FileSystem.OpenAppPackageFileAsync($"{lang}/states.json");
                var states = await JsonSerializer.DeserializeAsync<IEnumerable<BirthplaceDTO>>(statesStream);

                if (cities is not null && states is not null)
                {
                    birthplaces.AddRange(cities);
                    birthplaces.AddRange(states);
                }
            }
        }

        return birthplaces;
    }
}
