using System.Text.Json;

using MudBlazor;

namespace FiscalCode.Configuration;
public static class Config
{
    public static async Task<string?> GetServiceApiTokenAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("secrets.json");
        return stream != null ? (await JsonSerializer.DeserializeAsync<Secret>(stream))?.ServiceApiKey : null;
    }

    public static DialogOptions DialogOptions { get; } = new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Small,
        Position = DialogPosition.TopCenter
    };
}
