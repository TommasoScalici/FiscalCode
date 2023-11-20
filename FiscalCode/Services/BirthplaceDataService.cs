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
            using var stream = await FileSystem.OpenAppPackageFileAsync("cities.json");
            var cities = await JsonSerializer.DeserializeAsync<IEnumerable<BirthplaceDTO>>(stream);
            var currentCulture = CultureInfo.CurrentCulture;
            var currentUICulture = CultureInfo.CurrentUICulture;
        }

        return birthplaces;
    }
}
