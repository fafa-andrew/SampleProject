using BusinessEntities;
using Core.Services.Orders.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products.Contracts
{
    public interface IUpdateProductService
    {
        Task<Product> UpdateAsync(
            Guid id,
            string name,
            string description,
            decimal price,
            int stock,
            CancellationToken ct);

        Task AdjustStockAsync(List<OrderLineItem> items, StockAdjustment adjustment, CancellationToken ct);
    }
}
