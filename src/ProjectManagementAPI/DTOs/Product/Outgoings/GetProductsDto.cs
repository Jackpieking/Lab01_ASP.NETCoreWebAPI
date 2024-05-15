namespace ProjectManagementAPI.DTOs.Product.Outgoings;

public sealed class GetProductsDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int CategoryId { get; set; }

    public int UnitsInStock { get; set; }

    public decimal UnitPrice { get; set; }
}
