using System.Globalization;
using System.Text.RegularExpressions;

using FiscalCode.Data;
using FiscalCode.Features;

using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace FiscalCode.Services;
public class FiscalCodeOCRAnalyzerService(BirthplaceDataService birthplaceDataService)
{
    private readonly BirthplaceDataService birthplaceDataService = birthplaceDataService;
    private readonly string[] wordsToRemove =
        [
            "REPUBBLICA",
            "ITALIANA",
            "TESSERA",
            "SANITARIA",
            "CARTA",
            "REGIONALE",
            "NAZIONALE",
            "DEI",
            "DI",
            "SERVIZI",
            "MINISTERO",
            "DELL'INTERNO",
            "D'IDENTITÀ",
            "IDENTITY",
            "CARD",
            "SURNAME",
            "NAME",
            "ISSUING",
            "NATIONALITY",
            "EXPIRY",
            "SIGNATURE"
        ];
    private readonly Dictionary<string, string> fieldsToLookFor = new()
    {
        { "Nome", nameof(FiscalCodeDTO.FirstName)},
        { "Cognome", nameof(FiscalCodeDTO.LastName)},
        { "Sesso", nameof(FiscalCodeDTO.Sex)},
        { "Luogo", nameof(FiscalCodeDTO.BirthPlace)},
        { "Data", nameof(FiscalCodeDTO.BirthDate)},
        { "Codice", nameof(FiscalCodeDTO.FiscalCode)}
    };


    public async Task<FiscalCodeDTO> GetFiscalCodeDTOFromOCRScanAsync(IList<Line> resultLines)
    {
        var fiscalCodeDTO = new FiscalCodeDTO();
        var fiscalCodeDTOType = fiscalCodeDTO.GetType();
        var birthplaces = await birthplaceDataService.GetBirthplacesAsync();
        var allAtomicStrings = resultLines.SelectMany(res => res.Text.Split(" "));
        var allCapsResults = allAtomicStrings.Where(res => res.IsAllUpper());
        var isIDCard = false;
        var ignoreCase = StringComparison.OrdinalIgnoreCase;

        if (allCapsResults.Any(r => r.Contains("MINISTERO") || r.Contains("DELL'INTERNO")))
            isIDCard = true;

        foreach (var field in fieldsToLookFor.Keys)
        {
            var value = string.Empty;
            var potentialMatch = resultLines.FirstOrDefault(l =>
                                           l.Text.Split(" ").Any(s =>
                                           s.RemoveSpecialCharacters().Equals(field, ignoreCase)));

            if (potentialMatch == null) // search failed, we look for the next field
                continue;

            var indexPotentialMatch = resultLines.IndexOf(potentialMatch);
            var textPotentialMatch = potentialMatch.Text;

            // If a potential match is found...
            if (!string.IsNullOrEmpty(textPotentialMatch))
            {
                // First we manage the two "special" cases: Birthdate and Birthplace

                #region Birthplace management
                // Here we discard the potential match and we look for the exact match
                // filtering the birthplaces data source with all CAPS entries (which are values)
                if (field == "Luogo")
                {
                    var placesNames = birthplaces.Select(b => b.Name);
                    var bestMatch = string.Empty;
                    var bestLength = 0;

                    if (placesNames != null)
                    {
                        var capsResultsFiltered = allCapsResults.Where(x => x.Length > 1).ToList();
                        capsResultsFiltered.RemoveAll(res => wordsToRemove.Any(word =>
                                                            res.Contains(word, ignoreCase)));

                        // If we are scanning an ID card we look for a "cityname (province)" match
                        if (isIDCard)
                        {
                            var filteredResultLines = resultLines.Where(res => res.Text.Length > 1).ToList();
                            filteredResultLines.RemoveAll(line => wordsToRemove.Any(word => line.Text.Contains(word)));

                            foreach (var line in filteredResultLines)

                            {
                                var pattern = "[A-Za-z]+\\s\\([A-Za-z]{2}\\)";
                                var placeMatch = Regex.Match(line.Text, pattern);

                                if (placeMatch.Success)
                                {
                                    bestMatch = placeMatch.Value.Split(" ")[0];
                                    break;
                                }
                            }
                        }

                        // otherwise we look for the longest matching string
                        else
                        {
                            foreach (var capsResult in capsResultsFiltered)
                            {
                                var match = placesNames.FirstOrDefault(p =>
                                                  p.Contains(capsResult, ignoreCase));

                                if (!string.IsNullOrEmpty(match) && capsResult.Length > bestLength)
                                {
                                    bestMatch = match;
                                    bestLength = capsResult.Length;
                                }

                            }
                        }
                    }

                    fiscalCodeDTO.BirthPlace = birthplaces.Single(b =>
                                               b.Name.Equals(bestMatch, ignoreCase));
                }
                #endregion

                #region Birthdate management
                else if (field == "Data")
                {
                    var dates = new List<DateTime>();

                    foreach (var item in allAtomicStrings)
                    {
                        if (DateTime.TryParse(item, out var date))
                            dates.Add(date);
                    }

                    fiscalCodeDTO.BirthDate = dates.Min();
                }
                #endregion

                #region Other cases management
                else
                {
                    // The string is splitted and we look for value in the string
                    var splittedText = textPotentialMatch.Split(" ");

                    // If there are two entry (not containing special characters),
                    // the first should the field name and second should be the value
                    // otherwise we look for the next line
                    value = splittedText.Length == 2 && textPotentialMatch.IsAlphanumeric() ?
                            splittedText[1] :
                            resultLines[indexPotentialMatch + 1].Text;


                    if (isIDCard)
                    {
                        // adjustement to ID card structure, very workaroundish
                        if (field == "Sesso")
                            value = resultLines.SingleOrDefault(res => res.Text is "M" or "F")?.Text;
                    }

                    // The value is setted to DTO through reflection
                    var propertyName = fieldsToLookFor[field];

                    if (fiscalCodeDTOType != null && !string.IsNullOrEmpty(propertyName))
                    {
                        var property = fiscalCodeDTOType.GetProperty(propertyName);

                        if (property != null)
                        {
                            if (property.PropertyType == typeof(string))
                                property.SetValue(fiscalCodeDTO, value);
                        }
                    }
                }
                #endregion
            }
        }

        var textInfo = new CultureInfo("en", false).TextInfo;
        fiscalCodeDTO.FirstName = textInfo.ToTitleCase(fiscalCodeDTO.FirstName.ToLowerInvariant());
        fiscalCodeDTO.LastName = textInfo.ToTitleCase(fiscalCodeDTO.LastName.ToLowerInvariant());
        return fiscalCodeDTO;
    }
}