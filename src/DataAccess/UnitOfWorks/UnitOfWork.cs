using DataAccess.Data;
using DataAccess.Repositories.Category;
using DataAccess.Repositories.Product;

namespace DataAccess.UnitOfWorks;

public sealed class UnitOfWork : IUnitOfWork
{
    private IProductRepository _productRepository;
    private ICategoryRepository _categoryRepository;
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public IProductRepository ProductRepository
    {
        get { return _productRepository ??= new ProductRepository(context: _context); }
    }

    public ICategoryRepository CategoryRepository
    {
        get { return _categoryRepository ??= new CategoryRepository(context: _context); }
    }
}
