using BusinessEntities;
using Common;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct)
            => await _productRepository.GetAll.ToListAsync(ct);

        public async Task<Product> GetAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var product = await _productRepository.FindAsync(id);
            if (product != null) return product;

            throw new ArgumentException($"Product with ID {id} not found.", nameof(id));
        }
    }
}
