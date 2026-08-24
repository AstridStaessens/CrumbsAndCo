using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseContract>> GetAllAsync();
        Task<ProductResponseContract?> GetByIdAsync(int id);
        Task<IEnumerable<ProductResponseContract>> GetByCategoryAsync(int categoryId);
        Task<int> CreateAsync(CreateProductRequestContract contract);
        Task UpdateAsync(int id, UpdateProductRequestContract contract);
        Task DeleteAsync(int id);
    }
}
