using BusinessEntities;
using Common;
using Data.Repositories.Contracts;

namespace Data.Repositories
{
    [AutoRegister]
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(AppDbContext db) : base(db) { }
    }
}
