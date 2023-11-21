namespace FiscalCode.Data;
internal class Data
{
    public string CF { get; set; } = string.Empty;
    public List<string> All_CF { get; set; } = [];
}

internal class FiscalCodeAPIResponse
{
    public bool Status { get; set; }
    public string Messsage { get; set; } = string.Empty;
    public Data Data { get; set; } = new();
}
