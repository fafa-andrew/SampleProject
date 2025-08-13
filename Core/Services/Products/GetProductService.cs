using BusinessEntities;
using Common;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class GetProductService : IGetProductService
    {
        private readonly IProductRepository _productRepository;
        public GetProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public IQueryable<Product> GetAll() => _productRepository.GetAll;

        public async Task<Product> GetAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var product = await _productRepository.FindAsync(id);
            return product;
        }
    }
}
