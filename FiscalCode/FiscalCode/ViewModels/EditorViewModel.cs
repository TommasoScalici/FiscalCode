using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using FiscalCode.DataModels;
using FiscalCode.Localization;
using FiscalCode.Utilities;
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
            LoadData();

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
        public IEnumerable<District> Districts { get; private set; }
        public IEnumerable<Nation> Nations { get; private set; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand<object> SelectedItemCommand { get; }


        void LoadData()
        {
            var districtsList = new List<District>();
            var nationsList = new List<Nation>();
            var districtsStream = DataUtility.LoadStreamFromResource("DistrictsList");
            var nationsStream = DataUtility.LoadStreamFromResource("NationsList");

            using (var reader = new StreamReader(districtsStream))
            {
                while (!reader.EndOfStream)
                {
                    var lineSplitted = reader.ReadLine().Split(';');
                    var district = new District(lineSplitted[0], lineSplitted[1], lineSplitted[2]);
                    districtsList.Add(district);
                }
            }

            using (var reader = new StreamReader(nationsStream))
            {
                while (!reader.EndOfStream)
                {
                    var lineSplitted = reader.ReadLine().Split(';');
                    var nation = new Nation(lineSplitted[0], lineSplitted[1]);
                    nationsList.Add(nation);
                }
            }

            Districts = districtsList.Skip(4000);
            Nations = nationsList;
        }

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

            person.FiscalCode = FiscalCodeCalculator.Calculate(person);

            var mainVM = DependencyService.Get<MainViewModel>();
            mainVM.People.Add(person);
        }
    }
}
