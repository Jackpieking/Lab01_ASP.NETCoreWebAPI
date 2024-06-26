﻿namespace DataAccess.Entities;

public sealed class ProductEntity
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public int CategoryId { get; set; }

    public int UnitsInStock { get; set; }

    public decimal UnitPrice { get; set; }

    public CategoryEntity Category { get; set; }
}
