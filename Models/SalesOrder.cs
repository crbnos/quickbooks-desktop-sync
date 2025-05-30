using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("salesOrder")]
public class SalesOrder : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("salesOrderId")]
    public string SalesOrderId { get; set; }

    [Column("revisionId")]
    public int RevisionId { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("orderDate")]
    public DateTime OrderDate { get; set; }

    [Column("currencyCode")]
    public string CurrencyCode { get; set; }

    [Column("customerId")]
    public string CustomerId { get; set; }

    [Column("customerLocationId")]
    public string? CustomerLocationId { get; set; }

    [Column("customerContactId")]
    public string? CustomerContactId { get; set; }

    [Column("customerReference")]
    public string? CustomerReference { get; set; }

    [Column("assignee")]
    public string? Assignee { get; set; }

    [Column("companyId")]
    public string CompanyId { get; set; }

    [Column("closedAt")]
    public DateTime? ClosedAt { get; set; }

    [Column("closedBy")]
    public string? ClosedBy { get; set; }

    [Column("customFields")]
    public Dictionary<string, object>? CustomFields { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("createdBy")]
    public string CreatedBy { get; set; }

    [Column("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("locationId")]
    public string? LocationId { get; set; }

    [Column("exchangeRate")]
    public decimal? ExchangeRate { get; set; }

    [Column("exchangeRateUpdatedAt")]
    public DateTime? ExchangeRateUpdatedAt { get; set; }

    [Column("externalNotes")]
    public Dictionary<string, object>? ExternalNotes { get; set; }

    [Column("internalNotes")]
    public Dictionary<string, object>? InternalNotes { get; set; }

    [Column("salesPersonId")]
    public string? SalesPersonId { get; set; }

    [Column("sentCompleteDate")]
    public DateTime? SentCompleteDate { get; set; }

    [Column("opportunityId")]
    public string? OpportunityId { get; set; }

    [Column("completedDate")]
    public DateTime? CompletedDate { get; set; }
}
