using DataAccess.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Category;

public interface ICategoryRepository
{
    Task<bool> IsCategoryFoundByCategoryIdAsync(int categoryId, CancellationToken ct);

    Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken ct);
}
