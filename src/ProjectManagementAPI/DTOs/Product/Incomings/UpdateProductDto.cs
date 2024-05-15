using System.ComponentModel.DataAnnotations;

namespace ProjectManagementAPI.DTOs.Product.Incomings;

public sealed class UpdateProductDto
{
    [Required]
    [MinLength(length: 2)]
    public string ProductName { get; set; }

    [Required]
    [Range(minimum: default, maximum: int.MaxValue)]
    public int CategoryId { get; set; }

    [Required]
    [Range(minimum: default, maximum: 2_000_000)]
    public int UnitsInStock { get; set; }

    [Required]
    [Range(minimum: default, maximum: 2_000_000)]
    public decimal UnitPrice { get; set; }
}
