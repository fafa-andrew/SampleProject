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
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IIdObjectFactory<OrderItem> _orderItemFactory;
        private readonly IOrderRepository _orderRepository;

        public CreateOrderService(
            IOrderRepository orderRepository,
            IIdObjectFactory<Order> orderFactory,
            IIdObjectFactory<OrderItem> orderItemFactory)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _orderFactory = orderFactory ?? throw new ArgumentNullException(nameof(orderFactory));
            _orderItemFactory = orderItemFactory ?? throw new ArgumentNullException(nameof(orderItemFactory));
        }

        public async Task CreateAsync(
            string customerName, 
            DateTime orderDate, 
            List<OrderItemRequest> items, 
            CancellationToken ct)
        {
            var order = _orderFactory.Create(Guid.NewGuid());
            order.SetCustomerName(customerName);
            order.SetOrderDate(orderDate);
            order.SetStatus(OrderStatus.New);

            foreach (var item in items)
            {
                var orderItem = _orderItemFactory.Create();
                orderItem.SetProductId(item.ProductId);
                orderItem.SetQuantity(item.Quantity);
                orderItem.SetUnitPrice(item.UnitPrice);

                order.AddItem(orderItem);
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveAsync(ct);
        }
    }
}
