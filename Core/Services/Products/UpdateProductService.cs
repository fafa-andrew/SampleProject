using BusinessEntities;
using Common;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using System;
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
            _productRepository = productRepository ?? throw new System.ArgumentNullException(nameof(productRepository));
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

            await _productRepository.SaveAsync(ct);

            return product;
        }
    }
}
