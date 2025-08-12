using BusinessEntities;
using Common;
using Core.Services.Orders.Contracts;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Order> GetAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var order = await _orderRepository.FindAsync(id);
            if( order != null) return order;

            throw new ArgumentException($"Order with ID {id} not found.", nameof(id));
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct) 
            => await _orderRepository.GetAll.ToListAsync(ct);
    }
}
