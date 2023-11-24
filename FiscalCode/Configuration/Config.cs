using System.Text.Json;

using MudBlazor;

namespace FiscalCode.Configuration;
public static class Config
{
    public static Secret? Secrets { get; private set; }

    public static async Task<string?> GetAzureVisionOCRKeyAsync()
    {
        if (Secrets == null)
            await DeserializeSecretsAsync();

        return Secrets?.AzureVisionOCRKey;
    }

    public static async Task<string?> GetServiceApiTokenAsync()
    {
        if (Secrets == null)
            await DeserializeSecretsAsync();

        return Secrets?.ServiceApiKey;
    }

    public static DialogOptions DialogOptions { get; } = new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Small,
        Position = DialogPosition.TopCenter
    };

    private static async Task DeserializeSecretsAsync()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("secrets.json");
        Secrets = await JsonSerializer.DeserializeAsync<Secret>(stream);
    }
}
