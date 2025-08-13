using BusinessEntities;
using Core.Services.Orders.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface IUpdateOrderService
    {
        Task<Order> UpdateAsync(
            Guid id,
            string customerName, 
            List<OrderLineItem> items,
            CancellationToken ct);


        Task UpdateStatusAsync(Guid orderId, OrderStatus status, CancellationToken ct);
    }
}
