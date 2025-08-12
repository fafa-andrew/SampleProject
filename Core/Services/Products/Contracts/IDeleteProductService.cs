using System;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products.Contracts
{
    public interface IDeleteProductService
    {
        Task DeleteAsync(Guid id, CancellationToken ct);
    }
}
