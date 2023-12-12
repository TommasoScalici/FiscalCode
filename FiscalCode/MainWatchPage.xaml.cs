using System.Collections.ObjectModel;

using FiscalCode.Data;
using FiscalCode.Services;

namespace FiscalCode;

public partial class MainWatchPage : ContentPage
{
    public ObservableCollection<FiscalCodeDTO> FiscalCodes { get; private set; } = [];


    public MainWatchPage()
    {
        InitializeComponent();
        Task.Run(RefreshFiscalCodesAsync);
    }


    private async void ListViewRefreshing(object sender, EventArgs e)
    {
        await RefreshFiscalCodesAsync();
        listView.EndRefresh();
    }

    private async Task RefreshFiscalCodesAsync()
    {
        var fiscalCodes = await FiscalCodeDataService.LoadFiscalCodesFromFileAsync();

        foreach (var item in fiscalCodes)
            FiscalCodes.Add(item);
    }
}