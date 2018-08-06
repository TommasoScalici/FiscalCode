using FiscalCode.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CardPage : ContentPage
    {
        EditorViewModel editorVM;


        public CardPage() => InitializeComponent();

        public CardPage(EditorViewModel editorVM)
            : this()
        {
            this.editorVM = editorVM;
            DrawTextOnCard();
        }


        void DrawTextOnCard()
        {
            var assembly = GetType().Assembly;
            var stream = assembly.GetManifestResourceStream("FiscalCode.Resources.TesseraSanitaria.jpg");
        }
    }
}