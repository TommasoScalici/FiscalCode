using MudBlazor;

using Colors = MudBlazor.Colors;

namespace FiscalCode.Components.Layout;

public static class CustomMudTheme
{
    public static MudTheme GreenTheme { get; } = new MudTheme
    {
        Palette = new PaletteLight
        {
            Primary = Colors.Green.Default,
            Secondary = Colors.LightGreen.Default,
            AppbarBackground = Colors.Green.Darken4,
        },
        PaletteDark = new PaletteDark
        {
            Primary = Colors.Green.Lighten1,
            Secondary = Colors.LightGreen.Lighten1,
            AppbarBackground = Colors.Green.Darken4,
        }
    };
}
