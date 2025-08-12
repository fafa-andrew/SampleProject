using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface IDeleteOrderService
    {
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
