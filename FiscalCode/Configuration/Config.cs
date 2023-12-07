using System.Text.Json;

using MudBlazor;

namespace FiscalCode.Configuration;
public static class Config
{
    public static Secret? Secrets { get; private set; }

    public static DialogOptions DialogOptions { get; } = new()
    {
        CloseButton = true,
        CloseOnEscapeKey = true,
        MaxWidth = MaxWidth.Small,
        Position = DialogPosition.TopCenter
    };


    public static async Task<string?> GetAzureVisionOCRKeyAsync()
    {
        if (Secrets is null)
            await DeserializeSecretsAsync();

        return Secrets?.AzureVisionOCRKey;
    }

    public static async Task<string?> GetServiceApiTokenAsync()
    {
        if (Secrets is null)
            await DeserializeSecretsAsync();

        return Secrets?.ServiceApiKey;
    }

    public static async Task<string?> GetGoogleApisSecretsJsonAsync()
    {
        if (Secrets is null)
            await DeserializeSecretsAsync();

        return Secrets?.GoogleApisSecretsJson;
    }

    private static async Task DeserializeSecretsAsync()
    {
        using var secretsStream = await FileSystem.OpenAppPackageFileAsync("secrets.json");
        using var googleApisStream = await FileSystem.OpenAppPackageFileAsync("google_apis_ocr.json");

        Secrets = await JsonSerializer.DeserializeAsync<Secret>(secretsStream);

        using var streamReader = new StreamReader(googleApisStream);

        if (Secrets is not null)
            Secrets.GoogleApisSecretsJson = await streamReader.ReadToEndAsync();
    }
}
