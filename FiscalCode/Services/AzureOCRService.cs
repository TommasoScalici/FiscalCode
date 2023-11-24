using System.Diagnostics;

using FiscalCode.Configuration;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;


namespace FiscalCode.Services;

public static class AzureOCRService
{
    private const string endpoint = "https://fiscalcode-ocr.cognitiveservices.azure.com/";

    public static async Task<ReadResult?> PerformOCRAsync(Stream imageStream)
    {
        try
        {
            var client = await AuthenticateAsync();
            return await ReadFileUrlAsync(client, imageStream);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return null;
    }

    private static async Task<ComputerVisionClient> AuthenticateAsync()
    {
        var apiKey = await Config.GetAzureVisionOCRKeyAsync();
        var apiKeySCC = new ApiKeyServiceClientCredentials(apiKey);
        var client = new ComputerVisionClient(apiKeySCC)
        {
            Endpoint = endpoint
        };

        return client;
    }

    private static async Task<ReadResult> ReadFileUrlAsync(ComputerVisionClient client, Stream stream)
    {
        stream.Position = 0;
        var textHeaders = await client.ReadInStreamAsync(stream);
        var operationLocation = textHeaders.OperationLocation;

        const int numberOfCharsInOperationId = 36;

        var operationId = operationLocation[^numberOfCharsInOperationId..];

        ReadOperationResult results;

        do
        {
            results = await client.GetReadResultAsync(Guid.Parse(operationId));
        }
        while (results.Status is OperationStatusCodes.Running or OperationStatusCodes.NotStarted);

        var azureOcrResult = results.AnalyzeResult.ReadResults[0];

        return azureOcrResult;
    }
}
