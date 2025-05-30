using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("purchaseOrderLine")]
public class PurchaseOrderLine : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("purchaseOrderId")]
    public string PurchaseOrderId { get; set; }

    [Column("purchaseOrderLineType")]
    public string PurchaseOrderLineType { get; set; }

    [Column("itemId")]
    public string? ItemId { get; set; }

    [Column("itemReadableId")]
    public string? ItemReadableId { get; set; }

    [Column("accountNumber")]
    public string? AccountNumber { get; set; }

    [Column("assetId")]
    public string? AssetId { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("purchaseQuantity")]
    public decimal? PurchaseQuantity { get; set; }

    [Column("quantityReceived")]
    public decimal? QuantityReceived { get; set; }

    [Column("quantityInvoiced")]
    public decimal? QuantityInvoiced { get; set; }

    [Column("supplierUnitPrice")]
    public decimal? SupplierUnitPrice { get; set; }

    [Column("inventoryUnitOfMeasureCode")]
    public string? InventoryUnitOfMeasureCode { get; set; }

    [Column("purchaseUnitOfMeasureCode")]
    public string? PurchaseUnitOfMeasureCode { get; set; }

    [Column("locationId")]
    public string? LocationId { get; set; }

    [Column("shelfId")]
    public string? ShelfId { get; set; }

    [Column("setupPrice")]
    public decimal? SetupPrice { get; set; }

    [Column("receivedComplete")]
    public bool ReceivedComplete { get; set; }

    [Column("invoicedComplete")]
    public bool InvoicedComplete { get; set; }

    [Column("requiresInspection")]
    public bool RequiresInspection { get; set; }

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

    [Column("customFields")]
    public Dictionary<string, object>? CustomFields { get; set; }

    [Column("conversionFactor")]
    public decimal? ConversionFactor { get; set; }

    [Column("tags")]
    public string[]? Tags { get; set; }

    [Column("internalNotes")]
    public Dictionary<string, object>? InternalNotes { get; set; }

    [Column("externalNotes")]
    public Dictionary<string, object>? ExternalNotes { get; set; }

    [Column("exchangeRate")]
    public decimal ExchangeRate { get; set; }

    [Column("supplierShippingCost")]
    public decimal SupplierShippingCost { get; set; }

    [Column("modelUploadId")]
    public string? ModelUploadId { get; set; }

    [Column("supplierTaxAmount")]
    public decimal SupplierTaxAmount { get; set; }

    [Column("quantityToReceive")]
    public decimal? QuantityToReceive { get; set; }

    [Column("quantityToInvoice")]
    public decimal? QuantityToInvoice { get; set; }

    [Column("unitPrice")]
    public decimal? UnitPrice { get; set; }

    [Column("supplierExtendedPrice")]
    public decimal? SupplierExtendedPrice { get; set; }

    [Column("extendedPrice")]
    public decimal? ExtendedPrice { get; set; }

    [Column("shippingCost")]
    public decimal? ShippingCost { get; set; }

    [Column("taxAmount")]
    public decimal? TaxAmount { get; set; }

    [Column("taxPercent")]
    public decimal? TaxPercent { get; set; }

    [Column("jobId")]
    public string? JobId { get; set; }

    [Column("jobOperationId")]
    public string? JobOperationId { get; set; }

    [Column("quantityShipped")]
    public decimal? QuantityShipped { get; set; }
}
