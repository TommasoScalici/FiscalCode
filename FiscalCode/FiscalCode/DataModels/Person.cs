using System;

namespace FiscalCode.DataModels
{
    public class Person
    {
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
