using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("purchaseOrder")]
public class PurchaseOrder : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("purchaseOrderId")]
    public string PurchaseOrderId { get; set; }

    [Column("revisionId")] 
    public int RevisionId { get; set; }

    [Column("status")]
    public string Status { get; set; }

    [Column("orderDate")]
    public DateTime? OrderDate { get; set; }

    [Column("supplierId")]
    public string SupplierId { get; set; }

    [Column("supplierLocationId")]
    public string? SupplierLocationId { get; set; }

    [Column("supplierContactId")]
    public string? SupplierContactId { get; set; }

    [Column("supplierReference")]
    public string? SupplierReference { get; set; }

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

    [Column("currencyCode")]
    public string? CurrencyCode { get; set; }

    [Column("exchangeRate")]
    public decimal? ExchangeRate { get; set; }

    [Column("exchangeRateUpdatedAt")]
    public DateTime? ExchangeRateUpdatedAt { get; set; }

    [Column("tags")]
    public string[]? Tags { get; set; }

    [Column("internalNotes")]
    public Dictionary<string, object>? InternalNotes { get; set; }

    [Column("externalNotes")]
    public Dictionary<string, object>? ExternalNotes { get; set; }

    [Column("supplierInteractionId")]
    public string SupplierInteractionId { get; set; }

    [Column("purchaseOrderType")]
    public string PurchaseOrderType { get; set; }

    [Column("jobId")]
    public string? JobId { get; set; }

    [Column("jobReadableId")]
    public string? JobReadableId { get; set; }
}

