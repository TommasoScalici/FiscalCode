using System.Text.Json;

namespace FiscalCode.Configuration;
public static class Config
{
    public static async Task<string?> GetServiceApiTokenAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("secrets.json");
        return stream != null ? (await JsonSerializer.DeserializeAsync<Secret>(stream))?.ServiceApiKey : null;
    }
}
