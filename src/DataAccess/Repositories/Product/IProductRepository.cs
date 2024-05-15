using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Product;

public interface IProductRepository
{
    Task<IEnumerable<Entities.Product>> GetProductsAsync(CancellationToken ct);
}
