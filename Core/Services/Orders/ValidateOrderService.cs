using Common;
using Core.Services.Orders.Contracts;
using Core.Services.Orders.Models;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class ValidateOrderService : IValidateOrderService
    {
        private readonly IProductRepository _productRepository;

        public ValidateOrderService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<ItemValidationResult> ValidateItemsAsync(List<OrderLineItem> items, CancellationToken ct)
        {
            var result = new ItemValidationResult();

            if (items is null || !items.Any())
            {
                result.AddError("At least one item is required.");
                return result;
            }

            var groupedItems = items
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) });

            if (groupedItems.Any(g => g.ProductId == Guid.Empty)) result.AddError("One or more productIds are empty.");
            if (groupedItems.Any(g => g.Qty <= 0)) result.AddError("Quantity must be greater than 0 for all items.");
            if (!result.Ok) return result;

            var ids = groupedItems.Select(g => g.ProductId).ToArray();
            var products = await _productRepository.GetAll
                .Where(p => ids.Contains(p.Id))
                .Select(p => new { p.Id, p.Name, p.Stock, p.Price })
                .ToListAsync(ct);

            var map = products.ToDictionary(p => p.Id);

            foreach (var item in groupedItems)
            {
                if (!map.TryGetValue(item.ProductId, out var p))
                {
                    result.AddError($"Product '{item.ProductId}' not found.");
                    continue;
                }

                if (item.Qty > p.Stock)
                {
                    result.AddError($"Insufficient stock for '{p.Name}'. Requested {item.Qty}, available {p.Stock}.");
                    continue;
                }

                var orderItem = new OrderLineItem(item.ProductId, item.Qty, p.Price);
                result.AddLine(orderItem);
            }

            return result;
        }
    }
}
