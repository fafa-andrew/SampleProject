using BusinessEntities;
using System;
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
    }
}
