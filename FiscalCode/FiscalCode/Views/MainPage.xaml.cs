using System.Collections.ObjectModel;
using System.Linq;

using FiscalCode.ViewModels;
using FiscalCodeCalculator;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FiscalCode.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            ViewModel = DependencyService.Get<MainViewModel>();
            BindingContext = this;
            InitializeComponent();

            ViewModel.AddCommand.Executed += (sender, e) => Navigation.PushAsync(new EditorPage(ViewModel.EditorViewModel));
            ViewModel.EditCommand.Executed += (sender, e) => Navigation.PushAsync(new EditorPage(ViewModel.EditorViewModel));
            ViewModel.ShowCardCommand.Executed += (sender, e) => Navigation.PushAsync(new CardPage(ViewModel.EditorViewModel));
            ViewModel.DeleteCommand.Executed += (sender, e) => UpdateItems();
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
