using System.Collections.ObjectModel;

using FiscalCode.Data;
using FiscalCode.Services;

namespace FiscalCode;

public partial class MainWatchPage : ContentPage
{
    public ObservableCollection<FiscalCodeDTO>? FiscalCodes { get; private set; } = [];


    public MainWatchPage()
    {
        InitializeComponent();
        Task.Run(async () =>
        {
            var fiscalCodes = await FiscalCodeDataService.LoadFiscalCodesFromFileAsync();

            foreach (var item in fiscalCodes)
                FiscalCodes.Add(item);
        });
    }
}