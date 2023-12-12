namespace FiscalCode;

public partial class BarcodePopup : ContentPage
{
    public BarcodePopup() => InitializeComponent();

    private async void CloseButtonClicked(object sender, EventArgs e)
    {
        var navigation = Application.Current?.MainPage?.Navigation;

        if (navigation is not null)
            await navigation.PopModalAsync();
    }
}
