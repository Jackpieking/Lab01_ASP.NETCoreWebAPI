using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataAccess.Data;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Category;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Entities.CategoryEntity> _categories;

    internal CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _categories = context.Set<Entities.CategoryEntity>();
    }

    public async Task<IEnumerable<CategoryEntity>> GetAllAsync(CancellationToken ct)
    {
        return await _categories
            .Select(category => new CategoryEntity
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
            })
            .ToListAsync(ct);
    }

    public Task<bool> IsCategoryFoundByCategoryIdAsync(int categoryId, CancellationToken ct)
    {
        return _categories.AnyAsync(
            predicate: category => category.CategoryId == categoryId,
            cancellationToken: ct
        );
    }
}
