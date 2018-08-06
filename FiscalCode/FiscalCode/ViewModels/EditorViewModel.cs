using System;
using System.Collections.Generic;
using System.Linq;

using FiscalCode.Commands;
using FiscalCode.Localization;
using FiscalCodeCalculator;
using PropertyChanged;
using Xamarin.Forms;

namespace FiscalCode.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public sealed class EditorViewModel
    {
        readonly long id;
        Person person;


        public EditorViewModel()
        {
            Birthdate = DateTime.Now;
            InitializeCommands();
        }

        public EditorViewModel(Person person)
        {
            this.person = person;
            id = person.ID;
            Name = person.Name;
            Surname = person.Surname;
            Sex = person.Sex;
            Birthdate = person.Birthdate;
            Birthdistrict = person.BirthDistrict;
            Birthnation = person.BirthNation;
            InitializeCommands();
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
        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand<object> SelectedItemCommand { get; private set; }


        void CalculateFiscalCode()
        {
            var mainVM = DependencyService.Get<MainViewModel>();
            var lastId = mainVM.People.Count == 0 ? 0 : mainVM.People.Max(p => p.ID);

            if (id == 0)
                person = new Person(lastId + 1);

            person.Birthdate = Birthdate;
            person.BirthDistrict = Birthdistrict;
            person.BirthNation = Birthnation ?? new Nation(string.Empty, Locale.Localize("Italy"));
            person.BirthplaceCode = Birthdistrict != null ? Birthdistrict?.Code : Birthnation?.Code;
            person.Name = Name ?? string.Empty;
            person.Surname = Surname ?? string.Empty;
            person.Sex = Sex;

            person.FiscalCode = Calculator.Calculate(person);

            if (!mainVM.People.Any(p => p.ID == id))
                mainVM.People.Add(person);

            mainVM.SaveData();
        }

        void InitializeCommands()
        {
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
    }
}
