using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("company")]
public class Company : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("name")] 
    public string Name { get; set; }

    [Column("taxId")]
    public string? TaxId { get; set; }

    [Column("addressLine1")]
    public string? AddressLine1 { get; set; }

    [Column("addressLine2")]
    public string? AddressLine2 { get; set; }

    [Column("city")]
    public string? City { get; set; }

    [Column("stateProvince")]
    public string? StateProvince { get; set; }

    [Column("postalCode")]
    public string? PostalCode { get; set; }

    [Column("countryCode")]
    public string? CountryCode { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("fax")]
    public string? Fax { get; set; }

    [Column("email")]
    public string? Email { get; set; }

    [Column("website")]
    public string? Website { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("baseCurrencyCode")]
    public string BaseCurrencyCode { get; set; }

    [Column("logoDarkIcon")]
    public string? LogoDarkIcon { get; set; }

    [Column("logoLightIcon")]
    public string? LogoLightIcon { get; set; }

    [Column("logoDark")]
    public string? LogoDark { get; set; }

    [Column("logoLight")]
    public string? LogoLight { get; set; }

    [Column("slackChannel")]
    public string? SlackChannel { get; set; }
}
