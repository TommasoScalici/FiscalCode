namespace FiscalCode.DataModels
{
    public class Nation
    {
        public Nation(string code, string name)
        {
            Code = code;
            Name = name;
        }


        public string Code { get; }
        public string Name { get; }
    }
}
