using System.Threading;
using System.Threading.Tasks;

namespace Core.Services.Products.Contracts
{
    public interface ICreateProductService
    {
        Task CreateAsync(
            string name,
            string description,
            decimal price, 
            int stock,
            CancellationToken ct);
    }
}
