using DataAccess.Data;

namespace DataAccess.Repositories.Category;

public sealed class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    internal CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
}
