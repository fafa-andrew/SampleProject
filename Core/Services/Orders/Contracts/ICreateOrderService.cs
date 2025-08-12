using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface ICreateOrderService
    {
        Task CreateAsync(
            string customerName, 
            DateTime orderDate, 
            List<Models.OrderItemRequest> orderItems, 
            CancellationToken ct);
    }
}
