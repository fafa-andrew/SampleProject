using BusinessEntities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface IGetOrderService
    {
        IQueryable<Order> GetAll();
        Task<Order> GetAsync(Guid id, CancellationToken ct);
    }
}
