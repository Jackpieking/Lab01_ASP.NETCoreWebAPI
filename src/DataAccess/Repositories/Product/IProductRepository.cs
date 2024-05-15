using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Product;

public interface IProductRepository
{
    Task<IEnumerable<Entities.Product>> GetProductsAsync(CancellationToken ct);

    Task<bool> CreateProductAsync(Entities.Product newProduct, CancellationToken ct);

    Task<bool> IsProductFoundByProductNameAsync(string productName, CancellationToken ct);

    Task<bool> IsProductFoundByProductIdAsync(int productId, CancellationToken ct);

    Task<bool> RemoveProductAsync(int productId, CancellationToken ct);

    Task<bool> UpdateProductAsync(Entities.Product updatedProduct, CancellationToken ct);
}
