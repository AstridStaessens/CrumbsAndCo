using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Models.Interfaces
{
    public interface IOrderRepository
    {
        Task<List<OrderEntity>> GetAllAsync();
        Task<OrderEntity?> GetByIdAsync(int id);
        Task<List<OrderEntity>> GetByUserIdAsync(string userId);
        Task<int> CreateAsync(OrderEntity entity);
        Task UpdateAsync(OrderEntity entity);
    }
}