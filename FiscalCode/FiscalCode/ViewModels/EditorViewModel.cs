using System;
using System.Collections.Generic;

using FiscalCode.Localization;
using FiscalCodeCalculator;
using PropertyChanged;
using TommasoScalici.MVVMExtensions.Commands;
using Xamarin.Forms;

namespace FiscalCode.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public sealed class EditorViewModel
    {
        public EditorViewModel()
        {
            Birthdate = DateTime.Now;

            ConfirmCommand = new RelayCommand(CalculateFiscalCode);
            SelectedItemCommand = new RelayCommand<object>(item =>
            {
                if (item is District)
                {
                    Birthdistrict = item as District;
                    Birthnation = null;
                }

                if (item is Nation)
                {
                    Birthdistrict = null;
                    Birthnation = item as Nation;
                }
            });
        }


        public bool BornInItaly { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Birthdate { get; set; }
        public District Birthdistrict { get; set; }
        public Nation Birthnation { get; set; }
        public IEnumerable<District> Districts { get; } = FiscalDataStore.Districts;
        public IEnumerable<Nation> Nations { get; } = FiscalDataStore.Nations;
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand<object> SelectedItemCommand { get; }


        void CalculateFiscalCode()
        {
            var person = new Person
            {
                Birthdate = Birthdate,
                BirthDistrict = Birthdistrict,
                BirthNation = Birthnation ?? new Nation(string.Empty, Locale.Localize("Italy")),
                BirthplaceCode = Birthdistrict != null ? Birthdistrict.Code : Birthnation.Code,
                Name = Name,
                Surname = Surname,
                Sex = Sex
            };

            person.FiscalCode = Calculator.Calculate(person);

            var mainVM = DependencyService.Get<MainViewModel>();
            mainVM.People.Add(person);
        }
    }
}
