using System.Globalization;

using FiscalCode.Data;
using FiscalCode.Features;

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
        { "Codice", nameof(FiscalCodeDTO.FiscalCode)},
        { "Cognome", nameof(FiscalCodeDTO.LastName)},
        { "Nome", nameof(FiscalCodeDTO.FirstName)},
        { "Luogo", nameof(FiscalCodeDTO.BirthPlace)},
        { "Data", nameof(FiscalCodeDTO.BirthDate)},
        { "Sesso", nameof(FiscalCodeDTO.Sex)}
    };

    public async Task<FiscalCodeDTO> GetFiscalCodeDTOFromOCRScanAsync(string text)
    {
        var fiscalCodeDTO = new FiscalCodeDTO();

        var birthplaces = await birthplaceDataService.GetBirthplacesAsync();
        var allAtomicStrings = text.Split(' ', '\n').ToList();
        var allCapsResults = allAtomicStrings.Where(res => res.IsAllUpper());
        var textRows = text.Split("\n").ToList();
        var foundValues = new List<string>();

        var indexField = 0;
        var indexValue = 0;
        var isIDCard = false;
        var ignoreCase = StringComparison.OrdinalIgnoreCase;

        if (allCapsResults.Any(r => r.Contains("MINISTERO") || r.Contains("DELL'INTERNO")))
            isIDCard = true;

        foreach (var field in fieldsToLookFor.Keys)
        {
            if (indexField == -1)
                indexField = 0;

            indexField = textRows.FindIndex(indexField, item => item.Contains(field, ignoreCase));
            var potentialValue = string.Empty;
            var isValueValid = false;


            switch (field)
            {
                case "Codice":
                    if (isIDCard)
                        break;
                    indexValue = textRows.FindIndex(indexField,
                                                item => item.Split(" ").Any(s => s.Length == 16));

                    var fiscalCode = indexValue == -1 ? null :
                                           textRows[indexValue].Split(" ")
                                                                     .FirstOrDefault(item => item.Length == 16);

                    if (fiscalCode is not null)
                    {
                        fiscalCodeDTO.FiscalCode = fiscalCode;
                        foundValues.Add(fiscalCode);
                    }

                    break;
                case "Cognome":
                    isValueValid = false;

                    var splittedLast = textRows[indexField].Split(" ");

                    if (splittedLast.Length == 2)
                    {
                        indexField++;
                        potentialValue = splittedLast[1];
                    }
                    else
                    {
                        while (!isValueValid)
                        {
                            indexValue = textRows.FindIndex(indexField + 1, item => item.IsAllUpper());
                            potentialValue = textRows[indexValue];

                            if (!foundValues.Contains(potentialValue))
                                isValueValid = true;

                            indexField++;
                        }
                    }

                    fiscalCodeDTO.LastName = potentialValue;
                    foundValues.Add(potentialValue);

                    break;
                case "Nome":
                    isValueValid = false;

                    var splittedFirst = textRows[indexField].Split(" ");

                    if (splittedFirst.Length == 2)
                    {
                        indexField++;
                        potentialValue = splittedFirst[1];
                    }
                    else
                    {
                        while (!isValueValid)
                        {
                            indexValue = textRows.FindIndex(indexField + 1, item => item.IsAllUpper());
                            potentialValue = textRows[indexValue];

                            if (!foundValues.Contains(potentialValue))
                                isValueValid = true;

                            indexField++;
                        }
                    }

                    fiscalCodeDTO.FirstName = potentialValue;
                    foundValues.Add(potentialValue);

                    break;
                case "Sesso":
                    indexValue = textRows.FindIndex(indexField,
                                                    item => item.Split(" ")
                                                                     .Any(s => s.IsAllUpper() && s.Length == 1));

                    if (indexValue == -1)
                    {
                        indexValue = textRows.FindIndex(0,
                                                    item => item.Split(" ")
                                                                     .Any(s => s.IsAllUpper() && s.Length == 1
                                                                     && (s == "M" || s == "F")));
                    }

                    potentialValue = textRows[indexValue].Split(" ")
                                                                     .FirstOrDefault(item => item.Length == 1);
                    if (potentialValue is not null)
                    {
                        foundValues.Add(potentialValue);
                        fiscalCodeDTO.Sex = potentialValue;
                    }

                    break;
                case "Luogo":
                    isValueValid = false;
                    var potentialValues = new List<string>();

                    while (!isValueValid)
                    {
                        indexValue = textRows.FindIndex(indexField, item => item.IsAllUpper());
                        potentialValues = [.. textRows[indexValue].Split(" ")];
                        var placesNames = birthplaces.Select(b => b.Name);

                        foreach (var item in potentialValues)
                        {
                            if (!foundValues.Contains(item))
                            {
                                potentialValue = placesNames.FirstOrDefault(p =>
                                                            p.Equals(item, ignoreCase));

                                if (!string.IsNullOrEmpty(potentialValue))
                                {
                                    isValueValid = true;
                                    foundValues.Add(potentialValue);
                                    fiscalCodeDTO.BirthPlace = birthplaces.Single(b =>
                                                               b.Name.Equals(potentialValue, ignoreCase));
                                    break;
                                }
                            }
                        }

                        indexField++;
                    }

                    break;
                case "Data":
                    var dates = new List<DateTime>();

                    foreach (var item in allAtomicStrings)
                    {
                        if (DateTime.TryParse(item, out var date))
                            dates.Add(date);
                    }

                    fiscalCodeDTO.BirthDate = dates.Min();
                    break;
                default:
                    break;
            }
        }

        var textInfo = new CultureInfo("en", false).TextInfo;
        fiscalCodeDTO.FirstName = textInfo.ToTitleCase(fiscalCodeDTO.FirstName.ToLowerInvariant());
        fiscalCodeDTO.LastName = textInfo.ToTitleCase(fiscalCodeDTO.LastName.ToLowerInvariant());
        return fiscalCodeDTO;
    }

}