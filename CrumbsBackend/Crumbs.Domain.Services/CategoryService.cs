using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Mappers.Categories;
using Crumbs.Domain.Models.Interfaces;
using Crumbs.Domain.Services.Interfaces;

namespace Crumbs.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _repository;

        public CategoryService(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CategoryResponseContract>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities
                .Select(e => CategoryMapper.EntityToModel(e))
                .Select(CategoryMapper.ModelToResponseContract);
        }

        public async Task<CategoryResponseContract?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var model = CategoryMapper.EntityToModel(entity);
            return CategoryMapper.ModelToResponseContract(model);
        }

        public async Task<int> CreateAsync(CreateCategoryRequestContract contract)
        {
            var model = CategoryMapper.ContractToDomain(contract);
            var entity = CategoryMapper.ModelToEntity(model);
            return await _repository.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, UpdateCategoryRequestContract contract)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;

            entity.Name = contract.Name;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}