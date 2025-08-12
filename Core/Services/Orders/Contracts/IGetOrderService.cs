using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface IGetOrderService
    {
        Task<Order> GetAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct);
    }
}
