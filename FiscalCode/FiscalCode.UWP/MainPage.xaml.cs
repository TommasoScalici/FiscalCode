namespace FiscalCode.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(new FiscalCode.App());
        }
    }
}
