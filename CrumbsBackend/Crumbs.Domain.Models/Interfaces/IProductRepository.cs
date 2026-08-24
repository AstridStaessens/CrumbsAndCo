using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Models.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductEntity>> GetAllAsync();
        Task<ProductEntity?> GetByIdAsync(int id);
        Task<List<ProductEntity>> GetByCategoryAsync(int categoryId);
        Task<int> CreateAsync(ProductEntity entity);
        Task UpdateAsync(ProductEntity entity);
        Task DeleteAsync(int id);
    }
}