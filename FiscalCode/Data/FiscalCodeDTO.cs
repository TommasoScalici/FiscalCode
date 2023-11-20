namespace FiscalCode.Data;

public class FiscalCodeDTO
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Sex { get; set; } = string.Empty;

    public string BirthPlace { get; set; } = string.Empty;

    public string BirthState { get; set; } = string.Empty;

    public DateTime? BirthDate { get; set; } = DateTime.Now;
}