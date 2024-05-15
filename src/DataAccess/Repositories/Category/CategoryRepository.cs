using System.Threading;
using System.Threading.Tasks;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Category;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Entities.Category> _categories;

    internal CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _categories = context.Set<Entities.Category>();
    }

    public Task<bool> IsCategoryFoundByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return _categories.AnyAsync(
            predicate: category => category.CategoryId == categoryId,
            cancellationToken: ct
        );
    }
}
