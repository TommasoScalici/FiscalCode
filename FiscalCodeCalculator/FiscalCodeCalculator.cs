using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FiscalCodeCalculator
{
    public static class Calculator
    {
        public static readonly char[] Alphabet = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();
        public static readonly char[] MonthsMapping = { 'A', 'B', 'C', 'D', 'E', 'H', 'L', 'M', 'P', 'R', 'S', 'T' };
        public static readonly IDictionary<char, short> OddCharactersMap = new Dictionary<char, short>
        {
            ['0'] = 1,
            ['1'] = 0,
            ['2'] = 5,
            ['3'] = 7,
            ['4'] = 9,
            ['5'] = 13,
            ['6'] = 15,
            ['7'] = 17,
            ['8'] = 19,
            ['9'] = 21,
            ['A'] = 1,
            ['B'] = 0,
            ['C'] = 5,
            ['D'] = 7,
            ['E'] = 9,
            ['F'] = 13,
            ['G'] = 15,
            ['H'] = 17,
            ['I'] = 19,
            ['J'] = 21,
            ['K'] = 2,
            ['L'] = 4,
            ['M'] = 18,
            ['N'] = 20,
            ['O'] = 11,
            ['P'] = 3,
            ['Q'] = 6,
            ['R'] = 8,
            ['S'] = 12,
            ['T'] = 14,
            ['U'] = 16,
            ['V'] = 10,
            ['W'] = 22,
            ['X'] = 25,
            ['Y'] = 24,
            ['Z'] = 23
        };


        public static string Calculate(Person person)
        {
            var fiscalCode = string.Empty;

            var day = person.Sex == "F" ? (person.Birthdate.Day + 40).ToString() : person.Birthdate.Day.ToString("0#");
            var month = MonthsMapping[person.Birthdate.Month - 1];
            var year = person.Birthdate.Year.ToString().Substring(2, 2);
            var birthdate = $"{year}{month}{day}";
            var birthplaceCode = person.BirthplaceCode;
            var name = CalculateNameOrSurnameCode(person).Trim();
            var surname = CalculateNameOrSurnameCode(person, false).Trim();

            var partialCheckDigit = 0;
            var partialResult = $"{surname}{name}{birthdate}{birthplaceCode}";

            for (var i = 0; i < partialResult.Length; i++)
            {
                if ((i + 1) % 2 == 0)
                {
                    if (int.TryParse(partialResult[i].ToString(), out var digit))
                        partialCheckDigit += digit;
                    else
                        partialCheckDigit += Alphabet.ToList().IndexOf(partialResult[i]);
                }

                else
                    partialCheckDigit += OddCharactersMap[partialResult[i]];
            }

            var finalCheckDigit = Alphabet[partialCheckDigit % 26];
            fiscalCode = string.Concat(partialResult, finalCheckDigit);

            if (fiscalCode.Length != 16)
                return string.Empty;

            return fiscalCode;
        }

        static string CalculateNameOrSurnameCode(Person person, bool isCalculatingName = true)
        {
            var nameOrSurname = isCalculatingName ? person.Name : person.Surname;
            var consonants = string.Concat(from char c in nameOrSurname.ToUpperInvariant()
                                           where Regex.Match(c.ToString(), "[^AIEOU]").Success
                                           select c);

            var vowels = string.Concat(from char c in nameOrSurname.ToUpperInvariant()
                                       where Regex.Match(c.ToString(), "[AEIOU]").Success
                                       select c);

            if (isCalculatingName && consonants.Length > 3)
                consonants = consonants.Remove(1, 1);

            var result = string.Concat(consonants, vowels).Replace(" ", string.Empty);

            if (result.Length > 3)
                result = result.Substring(0, 3);

            while (result.Length < 3)
                result += "X";

            return result;
        }
    }
}
