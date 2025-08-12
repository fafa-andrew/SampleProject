using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products.Contracts
{
    public interface IGetProductService
    {
        Task<Product> GetAsync(Guid id, CancellationToken ct);
        Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct);
    }
}
