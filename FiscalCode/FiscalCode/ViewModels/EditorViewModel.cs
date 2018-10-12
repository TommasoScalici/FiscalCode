using System;
using System.Collections.Generic;
using System.Linq;

using FiscalCode.Commands;
using FiscalCode.Localization;
using FiscalCodeCalculator;
using PropertyChanged;

namespace FiscalCode.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public sealed class EditorViewModel
    {
        readonly long id;
        MainViewModel mainViewModel;
        Person person;

        public EditorViewModel() => InitializeCommands();

#pragma warning disable RECS0154
        public EditorViewModel(MainViewModel mainViewModel) : this() => this.mainViewModel = mainViewModel;
#pragma warning restore RECS0154

        public EditorViewModel(MainViewModel mainViewModel, Person person)
            : this(mainViewModel)
        {
            this.person = person;
            id = person.ID;
            Name = person.Name;
            Surname = person.Surname;
            Sex = person.Sex;
            Birthdate = person.Birthdate;
            Birthdistrict = person.BirthDistrict;
            Birthnation = person.BirthNation;
        }


        public bool BornInItaly { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Birthdate { get; set; } = DateTime.Now;
        public District Birthdistrict { get; set; }
        public Nation Birthnation { get; set; }
        public IEnumerable<District> Districts { get; } = FiscalDataStore.Districts;
        public IEnumerable<Nation> Nations { get; } = FiscalDataStore.Nations;
        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand<object> SelectedItemCommand { get; private set; }


        void CalculateFiscalCode()
        {
            var lastId = mainViewModel.People.Count == 0 ? 0 : mainViewModel.People.Max(p => p.ID);

            if (id == 0)
                person = new Person { ID = lastId + 1 };

            person.Birthdate = Birthdate;
            person.BirthDistrict = Birthdistrict;
            person.BirthNation = Birthnation ?? new Nation(string.Empty, Locale.Localize("Italy"));
            person.BirthplaceCode = Birthdistrict != null ? Birthdistrict?.Code : Birthnation?.Code;
            person.Name = Name ?? string.Empty;
            person.Surname = Surname ?? string.Empty;
            person.Sex = Sex;

            person.FiscalCode = Calculator.Calculate(person);

            if (!mainViewModel.People.Any(p => p.ID == id))
                mainViewModel.People.Add(person);

            mainViewModel.SaveData();
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
