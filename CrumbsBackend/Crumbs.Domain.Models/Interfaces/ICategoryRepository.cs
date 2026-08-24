using Crumbs.Persistence.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crumbs.Domain.Models.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoryEntity>> GetAllAsync();
        Task<CategoryEntity?> GetByIdAsync(int id);
        Task<int> CreateAsync(CategoryEntity entity);
        Task UpdateAsync(CategoryEntity entity);
        Task DeleteAsync(int id);
    }
}
