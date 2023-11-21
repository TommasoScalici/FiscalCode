using System.Text.Json;

using FiscalCode.Configuration;
using FiscalCode.Data;

namespace FiscalCode.Services;
public class FiscalCodeDataService(HttpClient http)
{
    private static readonly JsonSerializerOptions options = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient http = http;


    public async Task<string> CalculateFiscalCodeAsync(FiscalCodeDTO dto)
    {
        http.BaseAddress = new("http://api.miocodicefiscale.com");

        if (dto.BirthDate == null)
            throw new NullReferenceException(nameof(dto.BirthDate));

        if (dto.BirthPlace == null)
            throw new NullReferenceException(nameof(dto.BirthPlace));

        var birthdate = dto.BirthDate.Value;
        var token = await Config.GetServiceApiTokenAsync();

        var queryString = $"calculate?lname={dto.LastName}&fname={dto.FirstName}&gender={dto.Sex}"
                            + $"&city={dto.BirthPlace.Name}&state={dto.BirthPlace.State}"
                            + $"&day={birthdate.Day}&month={birthdate.Month}&year={birthdate.Year}"
                            + $"&access_token={token}";

        var response = await http.GetAsync(queryString);
        var stream = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<FiscalCodeAPIResponse>(stream, options);

        return apiResponse == null ? throw new NullReferenceException(nameof(apiResponse)) : apiResponse.Data.CF;
    }
}
