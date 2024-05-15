using DataAccess.Repositories.Category;
using DataAccess.Repositories.Product;

namespace DataAccess.UnitOfWorks;

public interface IUnitOfWork
{
    IProductRepository ProductRepository { get; }

    ICategoryRepository CategoryRepository { get; }
}
