using System.Diagnostics;

using FiscalCode.Data;
using FiscalCode.Services;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace FiscalCode.Features;
public class PhotoScannerOCR(FiscalCodeOCRAnalyzerService fiscalCodeOCRAnalyzerService)
{
    private readonly FiscalCodeOCRAnalyzerService fiscalCodeOCRAnalyzerService = fiscalCodeOCRAnalyzerService;


    public static async Task<FileResult?> CapturePhotoAsync()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
                return await MediaPicker.Default.CapturePhotoAsync();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return null;
    }

    public static async Task<ReadResult?> PerformOCRScanAsync(Stream stream)
    {
        try
        {
            return await AzureOCRService.PerformOCRAsync(stream);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return null;
    }

    public async Task<FiscalCodeDTO> GetFiscalCodeDTOFromOCRScanAsync(ReadResult result)
    {
        try
        {
            return await fiscalCodeOCRAnalyzerService.GetFiscalCodeDTOFromOCRScanAsync(result.Lines);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return new FiscalCodeDTO();
    }
}
