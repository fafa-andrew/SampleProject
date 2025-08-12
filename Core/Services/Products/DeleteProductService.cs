using Common;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class DeleteProductService : IDeleteProductService
    {
        private readonly IProductRepository _productRepository;
    
        public DeleteProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }
        
        public async Task DeleteAsync(Guid productId, CancellationToken ct)
        {
             ct.ThrowIfCancellationRequested();

            await _productRepository.DeleteAsync(productId);
        }
    }
}
