using BusinessEntities;
using Common;
using Core.Services.Orders.Contracts;
using Data.Repositories.Contracts;
using System;
using System.Linq;
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
            return order;
        }

        public IQueryable<Order> GetAll() => _orderRepository.GetAll;
    }
}
