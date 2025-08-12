using Core.Services.Orders.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    internal interface IUpdateOrderService
    {
        Task UpdateAsync(
            Guid id,
            string customerName, 
            DateTime orderDate,
            List<OrderItemRequest> items,
            CancellationToken ct);
    }
}
