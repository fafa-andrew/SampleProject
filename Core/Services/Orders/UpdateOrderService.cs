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

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus status, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var order = 
                await _orderRepository.FindAsync(orderId) ??
                throw new InvalidOperationException($"Order with ID {orderId} not found.");

            if (order.Status == OrderStatus.Completed) throw new InvalidOperationException("Cannot cancel a completed order.");
            
            order.SetStatus(status);
            order.SetModifiedDate(DateTime.UtcNow);

            await _orderRepository.SaveAsync(ct);
        }

        public async Task<Order> UpdateAsync(
            Guid id, 
            string name, 
            List<OrderLineItem> items,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var order = 
                await _orderRepository.FindAsync(id) ?? 
                throw new InvalidOperationException($"Order with ID {id} not found.");

            order.SetCustomerName(name);

            if (items != null)
            {
                order.ClearItems();
                foreach (var item in items)
                {
                    var orderItem = _orderItemFactory.Create();
                    orderItem.SetProductId(item.ProductId);
                    orderItem.SetQuantity(item.Quantity);
                    orderItem.SetUnitPrice(item.UnitPrice);
                    order.SetModifiedDate(DateTime.UtcNow);

                    order.AddItem(orderItem);
                }
            }

            await _orderRepository.SaveAsync(ct);

            return order;
        }
    }
}
