using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Product;

public sealed class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Entities.ProductEntity> _products;

    internal ProductRepository(ApplicationDbContext context)
    {
        _context = context;
        _products = context.Set<Entities.ProductEntity>();
    }

    public async Task<IEnumerable<Entities.ProductEntity>> GetProductsAsync(CancellationToken ct)
    {
        return await _products
            .AsNoTracking()
            .Select(selector: product => new Entities.ProductEntity
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                CategoryId = product.CategoryId
            })
            .ToListAsync(cancellationToken: ct);
    }

    public async Task<bool> CreateProductAsync(Entities.ProductEntity newProduct, CancellationToken ct)
    {
        try
        {
            await _products.AddAsync(entity: newProduct, cancellationToken: ct);

            await _context.SaveChangesAsync(cancellationToken: ct);
        }
        catch
        {
            return false;
        }

        return true;
    }

    public Task<bool> IsProductFoundByProductNameAsync(string productName, CancellationToken ct)
    {
        return _products.AnyAsync(
            predicate: product => product.ProductName.Equals(productName),
            cancellationToken: ct
        );
    }

    public Task<bool> IsProductFoundByProductIdAsync(int productId, CancellationToken ct)
    {
        return _products.AnyAsync(
            predicate: product => product.ProductId == productId,
            cancellationToken: ct
        );
    }

    public async Task<bool> RemoveProductAsync(int productId, CancellationToken ct)
    {
        var dbResult = true;

        await _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    await _products
                        .Where(predicate: product => product.ProductId == productId)
                        .ExecuteDeleteAsync(cancellationToken: ct);

                    await transaction.CommitAsync(cancellationToken: ct);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken: ct);

                    dbResult = false;
                }
            });

        return dbResult;
    }

    public async Task<bool> UpdateProductAsync(
        Entities.ProductEntity updatedProduct,
        CancellationToken ct
    )
    {
        var dbResult = true;

        await _context
            .Database.CreateExecutionStrategy()
            .ExecuteAsync(operation: async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync(
                    cancellationToken: ct
                );

                try
                {
                    await _products
                        .Where(product => product.ProductId == updatedProduct.ProductId)
                        .ExecuteUpdateAsync(builder =>
                            builder
                                .SetProperty(
                                    product => product.ProductName,
                                    updatedProduct.ProductName
                                )
                                .SetProperty(product => product.UnitPrice, updatedProduct.UnitPrice)
                                .SetProperty(
                                    product => product.UnitsInStock,
                                    updatedProduct.UnitsInStock
                                )
                                .SetProperty(
                                    product => product.CategoryId,
                                    updatedProduct.CategoryId
                                )
                        );

                    await transaction.CommitAsync(cancellationToken: ct);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken: ct);

                    dbResult = false;
                }
            });

        return dbResult;
    }

    public Task<ProductEntity> FindProductByIdAsync(int productId, CancellationToken ct)
    {
        return _products.FirstOrDefaultAsync(product => product.ProductId == productId, ct);
    }
}
