using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Orders.Contracts;
using Core.Services.Orders.Models;
using Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IIdObjectFactory<OrderItem> _orderItemFactory;
        private readonly IOrderRepository _orderRepository;

        public UpdateOrderService(
            IOrderRepository orderRepository,
            IIdObjectFactory<OrderItem> orderItemFactory
            )
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _orderItemFactory = orderItemFactory ?? throw new ArgumentNullException(nameof(orderItemFactory));
        }

        public async Task UpdateAsync(
            Guid id, 
            string name, 
            DateTime orderDate,
            List<OrderItemRequest> items,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var order = 
                await _orderRepository.FindAsync(id) ?? 
                throw new InvalidOperationException($"Order with ID {id} not found.");

            order.SetCustomerName(name);
            order.SetOrderDate(orderDate);

            if (items != null)
            {
                order.ClearItems();
                foreach (var item in items)
                {
                    var orderItem = _orderItemFactory.Create();
                    orderItem.SetProductId(item.ProductId);
                    orderItem.SetQuantity(item.Quantity);
                    orderItem.SetUnitPrice(item.UnitPrice);

                    order.AddItem(orderItem);
                }
            }

            await _orderRepository.SaveAsync(ct);
        }
    }
}
