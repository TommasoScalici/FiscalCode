namespace FiscalCode;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = DeviceInfo.Current.Idiom == DeviceIdiom.Watch ? new MainWatchPage() : new MainPage();
    }
}
