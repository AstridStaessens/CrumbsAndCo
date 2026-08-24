using Crumbs.Domain.Models.Interfaces;
using Crumbs.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Crumbs.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CrumbsDbContext _context;

        public CategoryRepository(CrumbsDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryEntity>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<CategoryEntity?> GetByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<int> CreateAsync(CategoryEntity entity)
        {
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task UpdateAsync(CategoryEntity entity)
        {
            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Categories.FindAsync(id);
            if (entity == null) return;
            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}