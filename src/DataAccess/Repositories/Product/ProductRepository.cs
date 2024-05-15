using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Product;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Entities.Product> _products;

    internal ProductRepository(ApplicationDbContext context)
    {
        _context = context;
        _products = context.Set<Entities.Product>();
    }

    public async Task<IEnumerable<Entities.Product>> GetProductsAsync(CancellationToken ct)
    {
        return await _products
            .AsNoTracking()
            .Select(selector: product => new Entities.Product
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryId = product.CategoryId
            })
            .ToListAsync(cancellationToken: ct);
    }
}
