using System;
using System.Collections.ObjectModel;

using FiscalCode.DataModels;
using FiscalCode.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }


        public ObservableCollection<Person> Items { get; private set; } = new ObservableCollection<Person>();


        protected override void OnAppearing()
        {
            base.OnAppearing();

            var mainVM = DependencyService.Get<MainViewModel>();
            Items.Clear();

            foreach (var item in mainVM.People)
                Items.Add(item);
        }

        async void ListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await Navigation.PushAsync(new EditorPage());

            ((ListView)sender).SelectedItem = null;
        }

        async void NewToolbarItemClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EditorPage());
    }
}
