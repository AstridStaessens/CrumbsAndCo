using Crumbs.Domain.Models.Interfaces;
using Crumbs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crumbs.Persistence.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CrumbsDbContext _context;

        public PaymentRepository(CrumbsDbContext context)
        {
            _context = context;
        }

        public async Task<PaymentEntity?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<PaymentEntity?> GetByMolliePaymentIdAsync(string molliePaymentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.MolliePaymentId == molliePaymentId);
        }

        public async Task<int> CreateAsync(PaymentEntity entity)
        {
            _context.Payments.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(PaymentEntity entity)
        {
            _context.Payments.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}