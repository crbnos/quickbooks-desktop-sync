using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("item")]
public class Item : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("readableId")]
    public string ReadableId { get; set; }

    [Column("name")] 
    public string Name { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("type")]
    public string Type { get; set; }

    [Column("replenishmentSystem")]
    public string ReplenishmentSystem { get; set; } = "Buy";

    [Column("defaultMethodType")]
    public string? DefaultMethodType { get; set; } = "Buy";

    [Column("itemTrackingType")]
    public string ItemTrackingType { get; set; }

    [Column("unitOfMeasureCode")]
    public string? UnitOfMeasureCode { get; set; }

    [Column("active")]
    public bool Active { get; set; } = true;

    [Column("companyId")]
    public string? CompanyId { get; set; }

    [Column("createdBy")]
    public string CreatedBy { get; set; }

    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }

    [Column("updatedBy")]
    public string? UpdatedBy { get; set; }

    [Column("updatedAt")] 
    public DateTime? UpdatedAt { get; set; }

    [Column("assignee")]
    public string? Assignee { get; set; }

    [Column("modelUploadId")]
    public string? ModelUploadId { get; set; }

    [Column("externalId")]
    public Dictionary<string, object>? ExternalId { get; set; }

    [Column("thumbnailPath")]
    public string? ThumbnailPath { get; set; }

    [Column("notes")]
    public Dictionary<string, object>? Notes { get; set; } = new Dictionary<string, object>();

    [Column("trackingMethod")]
    public string? TrackingMethod { get; set; }

    [Column("embedding")]
    public float[]? Embedding { get; set; }

    [Column("revision")]
    public string? Revision { get; set; } = "0";

    [Column("readableIdWithRevision")]
    public string? ReadableIdWithRevision { get; set; }
}


