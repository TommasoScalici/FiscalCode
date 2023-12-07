using System.Diagnostics;

using FiscalCode.Data;
using FiscalCode.Services;

using Google.Cloud.Vision.V1;

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

    public static async Task<TextAnnotation?> PerformOCRScanAsync(Stream stream)
    {
        try
        {
            return await GoogleOCRService.PerformOCRAsync(stream);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return null;
    }

    public async Task<FiscalCodeDTO> GetFiscalCodeDTOFromOCRScanAsync(TextAnnotation textAnnotation)
    {
        try
        {
            return await fiscalCodeOCRAnalyzerService.GetFiscalCodeDTOFromOCRScanAsync(textAnnotation.Text);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return new FiscalCodeDTO();
    }
}
