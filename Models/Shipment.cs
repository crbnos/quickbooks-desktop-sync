using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("shipment")]
public class Shipment : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("shipmentId")]
    public string ShipmentId { get; set; }

    [Column("locationId")]
    public string? LocationId { get; set; }

    [Column("sourceDocument")]
    public string? SourceDocument { get; set; } // Enum type in DB

    [Column("sourceDocumentId")] 
    public string? SourceDocumentId { get; set; }

    [Column("sourceDocumentReadableId")]
    public string? SourceDocumentReadableId { get; set; }

    [Column("shippingMethodId")]
    public string? ShippingMethodId { get; set; }

    [Column("trackingNumber")]
    public string? TrackingNumber { get; set; }

    [Column("customerId")]
    public string? CustomerId { get; set; }

    [Column("status")]
    public string Status { get; set; } // Enum type in DB

    [Column("postingDate")]
    public DateTime? PostingDate { get; set; }

    [Column("postedBy")]
    public string? PostedBy { get; set; }

    [Column("invoiced")]
    public bool? Invoiced { get; set; }

    [Column("assignee")]
    public string? Assignee { get; set; }

    [Column("internalNotes")]
    public Dictionary<string, object>? InternalNotes { get; set; }

    [Column("externalNotes")]
    public Dictionary<string, object>? ExternalNotes { get; set; }

    [Column("opportunityId")]
    public string? OpportunityId { get; set; }

    [Column("companyId")]
    public string CompanyId { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("createdBy")]
    public string CreatedBy { get; set; }

    [Column("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("tags")]
    public string[]? Tags { get; set; }

    [Column("customFields")]
    public Dictionary<string, object>? CustomFields { get; set; }

    [Column("supplierId")]
    public string? SupplierId { get; set; }

    [Column("supplierInteractionId")]
    public string? SupplierInteractionId { get; set; }

    [Column("externalDocumentId")]
    public string? ExternalDocumentId { get; set; }
}
