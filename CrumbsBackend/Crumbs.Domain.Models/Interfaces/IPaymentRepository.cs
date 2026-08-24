using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Models.Interfaces
{
    public interface IPaymentRepository
    {
        Task<PaymentEntity?> GetByOrderIdAsync(int orderId);
        Task<PaymentEntity?> GetByMolliePaymentIdAsync(string molliePaymentId);
        Task<int> CreateAsync(PaymentEntity entity);
        Task UpdateAsync(PaymentEntity entity);
    }
}