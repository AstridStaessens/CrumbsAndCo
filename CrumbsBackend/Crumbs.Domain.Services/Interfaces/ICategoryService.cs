using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryResponseContract>> GetAllAsync();
        Task<CategoryResponseContract?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateCategoryRequestContract contract);
        Task UpdateAsync(int id, UpdateCategoryRequestContract contract);
        Task DeleteAsync(int id);
    }
}