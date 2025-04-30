using System.ComponentModel.DataAnnotations; // Add for validation attributes

namespace Break.Contracts.Requests;

// Using record for immutability and conciseness, suitable for DTOs
public record CreateItemRequest(
    [Required]
    [MaxLength(100)]
    string ProductCode,

    [MaxLength(100)]
    string Barcode, // Consider if this should be required or unique

    [Required]
    [MaxLength(100)]
    string ProductName,

    [MaxLength(2000)]
    string ProductDescription,

    [MaxLength(100)]
    string ProductCategory,

    [Range(0, int.MaxValue)]
    int ReorderQuantity,

    [Required]
    [Range(0.01, (double)decimal.MaxValue)] // Ensure positive price
    [DataType(DataType.Currency)]
    decimal UnitPrice,

    [Required]
    [Range(0, int.MaxValue)] // Ensure non-negative stock
    int QuantityInStock,

    [Range(0, int.MaxValue)]
    int MinimumStockLevel,

    [Range(0, int.MaxValue)]
    int MaximumStockLevel
);
