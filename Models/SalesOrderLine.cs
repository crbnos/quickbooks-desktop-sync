using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("salesOrderLine")]
public class SalesOrderLine : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("salesOrderId")]
    public string SalesOrderId { get; set; }

    [Column("salesOrderLineType")]
    public string SalesOrderLineType { get; set; }

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

    [Column("saleQuantity")]
    public decimal? SaleQuantity { get; set; }

    [Column("quantitySent")]
    public decimal? QuantitySent { get; set; }

    [Column("quantityInvoiced")]
    public decimal? QuantityInvoiced { get; set; }

    [Column("unitPrice")]
    public decimal? UnitPrice { get; set; }

    [Column("unitOfMeasureCode")]
    public string? UnitOfMeasureCode { get; set; }

    [Column("locationId")]
    public string? LocationId { get; set; }

    [Column("shelfId")]
    public string? ShelfId { get; set; }

    [Column("setupPrice")]
    public decimal? SetupPrice { get; set; }

    [Column("sentComplete")]
    public bool SentComplete { get; set; }

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

    [Column("status")]
    public string Status { get; set; }

    [Column("modelUploadId")]
    public string? ModelUploadId { get; set; }

    [Column("promisedDate")]
    public DateTime? PromisedDate { get; set; }

    [Column("addOnCost")]
    public decimal AddOnCost { get; set; }

    [Column("methodType")]
    public string MethodType { get; set; }

    [Column("exchangeRate")]
    public decimal? ExchangeRate { get; set; }

    [Column("shippingCost")]
    public decimal ShippingCost { get; set; }

    [Column("taxPercent")]
    public decimal TaxPercent { get; set; }

    [Column("internalNotes")]
    public Dictionary<string, object>? InternalNotes { get; set; }

    [Column("externalNotes")]
    public Dictionary<string, object>? ExternalNotes { get; set; }

    [Column("quantityToSend")]
    public decimal? QuantityToSend { get; set; }

    [Column("quantityToInvoice")]
    public decimal? QuantityToInvoice { get; set; }

    [Column("convertedAddOnCost")]
    public decimal? ConvertedAddOnCost { get; set; }

    [Column("convertedShippingCost")]
    public decimal? ConvertedShippingCost { get; set; }

    [Column("convertedUnitPrice")]
    public decimal? ConvertedUnitPrice { get; set; }

    [Column("sentDate")]
    public DateTime? SentDate { get; set; }
}
