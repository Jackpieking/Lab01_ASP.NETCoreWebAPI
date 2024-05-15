using System.Collections.Generic;

namespace DataAccess.Entities;

public sealed class CategoryEntity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public IEnumerable<ProductEntity> Products { get; set; }
}
