using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage
    {
        public EditorPage() => InitializeComponent();


        async void ConfirmToolbarItemClicked(object sender, EventArgs e) => await Navigation.PopAsync();
    }
}
