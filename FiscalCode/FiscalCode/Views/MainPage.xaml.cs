using FiscalCode.Commands;
using FiscalCode.Localization;
using FiscalCode.Utilities;
using FiscalCode.ViewModels;

using FiscalCodeCalculator;
using Syncfusion.ListView.XForms;

using System;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = new MainViewModel();
            BindingContext = this;

            InitializeComponent();
            ViewModel.AddCommand.Executed += (sender, e) => Navigation.PushAsync(new EditorPage(ViewModel.EditorViewModel));
            ViewModel.CopyCommand.Executed += (sender, e) => DependencyService.Get<IMessage>().ShortAlert(Locale.Localize("CopySuccesfulDescription"));
            ViewModel.EditCommand.Executed += (sender, e) => Navigation.PushAsync(new EditorPage(ViewModel.EditorViewModel));
            ViewModel.ShowCardCommand.Executed += (sender, e) => Navigation.PushAsync(new CardPage(e.Parameter as Person));

            ViewModel.DeletionRequested += ViewModelDeletionRequested;
        }


        public MainViewModel ViewModel { get; }
        public ObservableCollection<Person> Items { get; private set; } = new ObservableCollection<Person>();


        protected override void OnAppearing()
        {
            base.OnAppearing();
            UpdateItems();
            ViewModel.SelectedItems.Clear();
            UpdateUI();
        }


        async void ViewModelDeletionRequested(object sender, EventArgs e)
        {
            var result = await DisplayAlert(Locale.Localize("DeleteConfirmationTitle"), Locale.Localize("DeleteConfirmationMessage"),
                                            Locale.Localize("Yes"), Locale.Localize("No"));
            if (result)
            {
                ViewModel.DeleteSelectedItems();
                UpdateItems();
            }
        }

        void SfListViewSelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            if (sender is SfListView sfListView && sfListView.SelectedItems != null)
                ViewModel.SelectedItems = sfListView.SelectedItems.Cast<Person>().ToList();

            UpdateUI();
        }

        void UpdateItems()
        {
            Items.Clear();

            foreach (var item in ViewModel.People)
                Items.Add(item);
        }

        void UpdateUI()
        {
            ViewModel.DeleteCommand.RaiseCanExecuteChanged();
            ViewModel.EditCommand.RaiseCanExecuteChanged();
            ViewModel.ShowCardCommand.RaiseCanExecuteChanged();
        }
    }
}
