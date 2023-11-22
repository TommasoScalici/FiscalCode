using System.Diagnostics;
using System.Text.Json;

using FiscalCode.Configuration;
using FiscalCode.Data;

namespace FiscalCode.Services;
public class FiscalCodeDataService
{
    private static readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private static readonly string dataJsonFileName = "data.json";
    private static readonly string fullPath = $"{FileSystem.AppDataDirectory}{Path.DirectorySeparatorChar}{dataJsonFileName}";
    private static string? token;
    private readonly HttpClient http;


    public FiscalCodeDataService(HttpClient http)
    {
        this.http = http;
        this.http.BaseAddress = new("http://api.miocodicefiscale.com");
    }


    public static async Task<IEnumerable<FiscalCodeDTO>> LoadFiscalCodesFromFileAsync()
    {
        try
        {
            var json = await File.ReadAllBytesAsync(fullPath);
            using var stream = new MemoryStream(json);
            var list = await JsonSerializer.DeserializeAsync<IEnumerable<FiscalCodeDTO>>(stream);
            return list ?? Enumerable.Empty<FiscalCodeDTO>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }

        return Enumerable.Empty<FiscalCodeDTO>();
    }

    public static async Task SaveFiscalCodesToFileAsync(IEnumerable<FiscalCodeDTO> list)
    {
        try
        {
            var json = JsonSerializer.Serialize(list);
            await File.WriteAllTextAsync(fullPath, json);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }

    public async Task<string> CalculateFiscalCodeAsync(FiscalCodeDTO dto)
    {
        if (string.IsNullOrEmpty(token))
            token = await Config.GetServiceApiTokenAsync();

        if (token == null)
            throw new NullReferenceException(nameof(token));

        if (dto.BirthDate == null)
            throw new NullReferenceException(nameof(dto.BirthDate));

        if (dto.BirthPlace == null)
            throw new NullReferenceException(nameof(dto.BirthPlace));

        var birthdate = dto.BirthDate.Value;

        var queryString = $"calculate?lname={dto.LastName}&fname={dto.FirstName}&gender={dto.Sex}"
                            + $"&city={dto.BirthPlace.Name}&state={dto.BirthPlace.State}"
                            + $"&day={birthdate.Day}&month={birthdate.Month}&year={birthdate.Year}"
                            + $"&access_token={token}";

        var response = await http.GetAsync(queryString);
        using var stream = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<FiscalCodeAPIResponse>(stream, options);

        return apiResponse == null ? throw new NullReferenceException(nameof(apiResponse)) : apiResponse.Data.CF;
    }
}
