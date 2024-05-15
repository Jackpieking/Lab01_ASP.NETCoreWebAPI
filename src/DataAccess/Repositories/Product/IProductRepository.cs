using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Product;

public interface IProductRepository
{
    Task<IEnumerable<Entities.ProductEntity>> GetProductsAsync(CancellationToken ct);

    Task<bool> CreateProductAsync(Entities.ProductEntity newProduct, CancellationToken ct);

    Task<ProductEntity> FindProductByIdAsync(int productId, CancellationToken ct);

    Task<bool> IsProductFoundByProductNameAsync(string productName, CancellationToken ct);

    Task<bool> IsProductFoundByProductIdAsync(int productId, CancellationToken ct);

    Task<bool> RemoveProductAsync(int productId, CancellationToken ct);

    Task<bool> UpdateProductAsync(Entities.ProductEntity updatedProduct, CancellationToken ct);
}
