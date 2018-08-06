using System;

using FiscalCode.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage
    {
        public EditorPage()
            => InitializeComponent();

#pragma warning disable RECS0154 
        public EditorPage(EditorViewModel viewModel)
            : this() => BindingContext = viewModel;
#pragma warning restore RECS0154


        async void ConfirmToolbarItemClicked(object sender, EventArgs e) => await Navigation.PopAsync();
    }
}
