using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("shipmentLine")]
public class ShipmentLine : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("shipmentId")]
    public string ShipmentId { get; set; }

    [Column("lineId")] 
    public string? LineId { get; set; }

    [Column("itemId")]
    public string ItemId { get; set; }

    [Column("itemReadableId")]
    public string? ItemReadableId { get; set; }

    [Column("orderQuantity")]
    public decimal OrderQuantity { get; set; }

    [Column("outstandingQuantity")]
    public decimal OutstandingQuantity { get; set; }

    [Column("shippedQuantity")]
    public decimal ShippedQuantity { get; set; }

    [Column("locationId")]
    public string? LocationId { get; set; }

    [Column("shelfId")]
    public string? ShelfId { get; set; }

    [Column("unitOfMeasure")]
    public string UnitOfMeasure { get; set; }

    [Column("unitPrice")]
    public decimal UnitPrice { get; set; }

    [Column("requiresSerialTracking")]
    public bool RequiresSerialTracking { get; set; }

    [Column("requiresBatchTracking")]
    public bool RequiresBatchTracking { get; set; }

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

    [Column("fulfillmentId")]
    public string? FulfillmentId { get; set; }
}
