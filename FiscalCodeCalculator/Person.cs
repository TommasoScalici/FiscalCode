using System;

using PropertyChanged;

namespace FiscalCodeCalculator
{
    [AddINotifyPropertyChangedInterface]
    public class Person
    {
        public long ID { get; set; }
        public string BirthplaceCode { get; set; }
        public string FiscalCode { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Sex { get; set; }
        public DateTime Birthdate { get; set; }
        public District BirthDistrict { get; set; }
        public Nation BirthNation { get; set; }
    }
}
