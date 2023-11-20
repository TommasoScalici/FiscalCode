using FiscalCode.Configuration;
using FiscalCode.Data;

using Microsoft.Extensions.Configuration;

namespace FiscalCode.Services;
public class FiscalCodeDataService(IConfiguration config, HttpClient http)
{
    private readonly IConfiguration config = config;
    private readonly HttpClient http = http;


    public async Task<string> CalculateFiscalCodeAsync(FiscalCodeDTO dto)
    {
        http.BaseAddress = new("http://api.miocodicefiscale.com");

        var queryString = $"calculate?lname={dto.LastName}&fname={dto.FirstName}&gender={dto.Sex}&city={dto.BirthPlace}&state={dto.BirthState}"
                            + $"&day={dto.BirthDate.Value.Day}&month={dto.BirthDate.Value.Month}&year={dto.BirthDate.Value.Year}"
                            + $"&access_token={Config.ServiceApiKey}";

        var response = await http.GetAsync(queryString);

        return await response.Content.ReadAsStringAsync();
    }
}
