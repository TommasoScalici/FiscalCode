using System.Diagnostics;

using FiscalCode.Data;
using FiscalCode.Services;

namespace FiscalCode.Features;
public class PhotoScannerOCR(FiscalCodeOCRAnalyzerService fiscalCodeOCRAnalyzerService)
{
    private readonly FiscalCodeOCRAnalyzerService fiscalCodeOCRAnalyzerService = fiscalCodeOCRAnalyzerService;


    public async Task<FiscalCodeDTO> GetFiscalCodeDTOFromOCRScanAsync()
    {
        try
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                var photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    using var imageStream = await photo.OpenReadAsync();
                    var result = await AzureOCRService.PerformOCRAsync(imageStream);

                    if (result != null)
                        return await fiscalCodeOCRAnalyzerService.GetFiscalCodeDTOFromOCRScanAsync(result.Lines);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return new FiscalCodeDTO();
    }
}
