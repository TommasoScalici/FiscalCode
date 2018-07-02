namespace FiscalCodeCalculator
{
    public class District
    {
        public District(string code, string name, string provinceAbbreviation)
        {
            Code = code;
            Name = name;
            ProvinceAbbreviation = provinceAbbreviation;
        }


        public string Code { get; }
        public string Name { get; }
        public string ProvinceAbbreviation { get; }
    }
}
