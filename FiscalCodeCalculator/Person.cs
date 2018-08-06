using System;

using PropertyChanged;

namespace FiscalCodeCalculator
{
    [AddINotifyPropertyChangedInterface]
    public class Person
    {
#pragma warning disable RECS0154
        public Person() { }
        public Person(long id) => ID = id;
#pragma warning restore RECS0154


        public long ID { get; }
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
