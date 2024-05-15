﻿using System.Collections.Generic;
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

    public async Task<bool> CreateProductAsync(Entities.Product newProduct, CancellationToken ct)
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
}
