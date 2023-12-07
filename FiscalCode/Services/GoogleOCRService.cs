using FiscalCode.Configuration;

using Google.Api.Gax.Grpc.Rest;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;

using Image = Google.Cloud.Vision.V1.Image;

namespace FiscalCode.Services;
public class GoogleOCRService
{
    private static GoogleCredential? googleCredential;


    public static async Task<TextAnnotation> PerformOCRAsync(Stream imageStream)
    {
        var json = await Config.GetGoogleApisSecretsJsonAsync();

        if (googleCredential is null && json is not null)
            googleCredential = GoogleCredential.FromJson(json);

        var client = await new ImageAnnotatorClientBuilder
        {
            GoogleCredential = googleCredential,
            GrpcAdapter = RestGrpcAdapter.Default
        }.BuildAsync();

        var image = await Image.FromStreamAsync(imageStream);

        return await client.DetectDocumentTextAsync(image);
    }
}
