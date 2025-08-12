using Common;
using Core.Services.Orders.Contracts;
using Data.Repositories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class DeleteOrderService : IDeleteOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public DeleteOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            await _orderRepository.DeleteAsync(id);
        }
    }
}
