using System.Text;

namespace FiscalCode.Features;

public static class ExtensionMethods
{
    public static bool IsAlphanumeric(this char c) =>
        c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or ' ';

    public static bool IsAlphanumeric(this string input) =>
        input.All(c => c.IsAlphanumeric());

    public static bool IsAllUpper(this string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsLetter(input[i]) && !char.IsUpper(input[i]))
                return false;
        }

        return true;
    }

    public static string RemoveSpecialCharacters(this string input)
    {
        var sb = new StringBuilder();

        foreach (var c in input)
        {
            if (c.IsAlphanumeric())
                sb.Append(c);
        }

        return sb.ToString();
    }
}