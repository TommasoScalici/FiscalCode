namespace FiscalCode.Configuration;
public class Secret
{
    public string AzureVisionOCRKey { get; set; } = string.Empty;
    public string GoogleApisSecretsJson { get; set; } = string.Empty;
    public string ServiceApiKey { get; set; } = string.Empty;
    public static string AdUnitBanner { get; set; } = "ca-app-pub-2308630572499783/7989376672";
    public static string AdUnitInterstitial { get; set; } = "ca-app-pub-2308630572499783/1230511564";
}
