using BusinessEntities;
using Common;
using Data.Repositories.Contracts;

namespace Data.Repositories
{
    [AutoRegister]
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext db) : base(db) { }
    }
}
