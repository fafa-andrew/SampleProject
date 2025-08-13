using Core.Services.Orders.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders.Contracts
{
    public interface IValidateOrderService
    {
        Task<ItemValidationResult> ValidateItemsAsync(List<OrderLineItem> items, CancellationToken ct);
    }
}
