using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("customer")]
public class Customer : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("customerTypeId")]
    public string? CustomerTypeId { get; set; }

    [Column("customerStatusId")] 
    public string? CustomerStatusId { get; set; }

    [Column("taxId")]
    public string? TaxId { get; set; }

    [Column("accountManagerId")]
    public string? AccountManagerId { get; set; }

    [Column("logo")]
    public string? Logo { get; set; }

    [Column("assignee")]
    public string? Assignee { get; set; }

    [Column("companyId")]
    public string CompanyId { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("createdBy")]
    public string? CreatedBy { get; set; }

    [Column("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("customFields")]
    public Dictionary<string, object>? CustomFields { get; set; }

    [Column("currencyCode")]
    public string? CurrencyCode { get; set; }

    [Column("phone")]
    public string? Phone { get; set; }

    [Column("fax")]
    public string? Fax { get; set; }

    [Column("website")]
    public string? Website { get; set; }

    [Column("externalId")]
    public Dictionary<string, object>? ExternalId { get; set; }

    [Column("taxPercent")]
    public decimal TaxPercent { get; set; }

    [Column("tags")]
    public string[]? Tags { get; set; }

}
