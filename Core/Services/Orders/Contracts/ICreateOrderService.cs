using BusinessEntities;
using Core.Services.Orders.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface ICreateOrderService
    {
        Task<Order> CreateAsync(string customerName, List<OrderLineItem> items, CancellationToken ct);
    }
}
