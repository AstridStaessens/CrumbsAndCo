using Crumbs.Domain.Models.Interfaces;
using Crumbs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crumbs.Persistence.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CrumbsDbContext _context;

        public OrderRepository(CrumbsDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderEntity>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .ToListAsync();
        }

        public async Task<OrderEntity?> GetByIdAsync(int id)
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<List<OrderEntity>> GetByUserIdAsync(string userId)
        {
            return await _context.Orders
                .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> CreateAsync(OrderEntity entity)
        {
            _context.Orders.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(OrderEntity entity)
        {
            _context.Orders.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}