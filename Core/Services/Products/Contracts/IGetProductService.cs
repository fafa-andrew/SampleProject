using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products.Contracts
{
    public interface IGetProductService
    {
        Task<Product> GetAsync(Guid id, CancellationToken ct);
        IQueryable<Product> GetAll();
    }
}
