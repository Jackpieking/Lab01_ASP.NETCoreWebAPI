using System.Threading;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Category;

public interface ICategoryRepository
{
    Task<bool> IsCategoryFoundByCategoryIdAsync(int categoryId, CancellationToken ct);
}
