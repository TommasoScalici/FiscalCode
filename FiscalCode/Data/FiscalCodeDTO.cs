namespace FiscalCode.Data;

public class FiscalCodeDTO
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Sex { get; set; } = string.Empty;
    public string FiscalCode { get; set; } = string.Empty;

    public BirthplaceDTO? BirthPlace { get; set; }

    public DateTime? BirthDate { get; set; }

    public int Order { get; set; }
}