using System.Collections.Generic;

namespace DataAccess.Entities;

public sealed class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public IEnumerable<Product> Products { get; set; }
}
