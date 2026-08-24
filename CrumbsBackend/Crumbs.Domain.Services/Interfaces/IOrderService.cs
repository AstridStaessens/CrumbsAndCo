using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseContract>> GetAllAsync();
        Task<OrderResponseContract?> GetByIdAsync(int id);
        Task<IEnumerable<OrderResponseContract>> GetByUserIdAsync(string userId);
        Task<int> CreateAsync(string userId, CreateOrderRequestContract contract);
        Task UpdateStatusAsync(int id, string status);
        Task CancelAsync(string userId, int id);
    }
}