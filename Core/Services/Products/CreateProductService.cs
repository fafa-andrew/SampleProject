using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Products.Contracts;
using Data.Repositories.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products
{
    [AutoRegister]
    public class CreateProductService : ICreateProductService
    {
        private readonly IIdObjectFactory<Product> _productFactory;
        private readonly IProductRepository _productRepository;

        public CreateProductService(
            IIdObjectFactory<Product> productFactory, 
            IProductRepository productRepository)
        {
            _productFactory = productFactory ?? throw new ArgumentNullException(nameof(productFactory));
        }

        public async Task CreateAsync(
            string name, 
            string description, 
            decimal price, 
            int stock,
            CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            var product = _productFactory.Create(new Guid());
            
            product.SetName(name);
            product.SetDescription(description);
            product.SetPrice(price);
            product.SetStock(stock);

            await _productRepository.AddAsync(product);
            await _productRepository.SaveAsync(ct);
        }
    }
}
