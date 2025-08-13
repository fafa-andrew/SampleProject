using BusinessEntities;
using Common;
using Core.Services.Orders.Models;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class UpdateProductService : IUpdateProductService
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<Product> UpdateAsync(
            Guid id, 
            string name, 
            string description, 
            decimal price, 
            int stock, 
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var product = 
                await _productRepository.FindAsync(id) ??
                throw new InvalidOperationException($"Product with ID {id} not found.");

            product.SetName(name);
            product.SetDescription(description);
            product.SetPrice(price);
            product.SetStock(stock);
            product.SetModifiedDate(DateTime.UtcNow);

            await _productRepository.SaveAsync(ct);

            return product;
        }

        public async Task AdjustStockAsync(List<OrderLineItem> items, StockAdjustment adjutment, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            if (items == null || items.Count == 0) throw new ApplicationException("Items must exist");

            var groups = items
                .GroupBy(i => i.ProductId)
                .Select(g => new { ProductId = g.Key, Qty = g.Sum(x => x.Quantity) })
                .ToList();

            if (groups.Any(g => g.ProductId == Guid.Empty)) throw new ArgumentException("One or more productIds are empty.");
            if (groups.Any(g => g.Qty <= 0)) throw new ArgumentOutOfRangeException(nameof(items), "All quantities must be > 0.");

            var ids = groups.Select(g => g.ProductId).ToArray();
            var products = await _productRepository.GetAll.Where(x => ids.Contains(x.Id)).ToListAsync();

            var map = products.ToDictionary(p => p.Id);

            foreach (var g in groups)
            {
                if (!map.TryGetValue(g.ProductId, out var product))
                    throw new InvalidOperationException($"Product {g.ProductId} not found.");

                if (adjutment == StockAdjustment.Increase) product.IncreaseStock(g.Qty);
                else product.DecreaseStock(g.Qty);
            }

            await _productRepository.SaveAsync(ct);
        }

    }
}
