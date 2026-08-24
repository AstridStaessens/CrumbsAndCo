using Crumbs.API.Contracts.ResponseContracts;

namespace Crumbs.Domain.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResponseContract> CreatePaymentAsync(int orderId);
        Task HandleWebhookAsync(string molliePaymentId);
        Task<string> GetPaymentStatusAsync(int orderId);
        Task RefundAsync(int orderId);
    }
}