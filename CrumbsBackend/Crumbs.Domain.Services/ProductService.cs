using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Mappers.Products;
using Crumbs.Domain.Models.Interfaces;
using Crumbs.Domain.Services.Interfaces;

namespace Crumbs.Domain.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductResponseContract>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();

            return entities
                .Select(e => ProductMapper.EntityToModel(e))
                .Select(ProductMapper.ModelToResponseContract);
        }

        public async Task<ProductResponseContract?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            var model = ProductMapper.EntityToModel(entity);
            return ProductMapper.ModelToResponseContract(model);
        }

        public async Task<IEnumerable<ProductResponseContract>> GetByCategoryAsync(int categoryId)
        {
            var entities = await _repository.GetByCategoryAsync(categoryId);

            return entities
                .Select(e => ProductMapper.EntityToModel(e))
                .Select(ProductMapper.ModelToResponseContract);
        }

        public async Task<int> CreateAsync(CreateProductRequestContract contract)
        {
            var model = ProductMapper.ContractToDomain(contract);
            var entity = ProductMapper.ModelToEntity(model);
            return await _repository.CreateAsync(entity);
        }

        public async Task UpdateAsync(int id, UpdateProductRequestContract contract)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return;

            entity.Name = contract.Name;
            entity.Description = contract.Description;
            entity.Price = contract.Price;
            entity.Stock = contract.Stock;
            entity.ImageUrl = contract.ImageUrl;
            entity.CategoryId = contract.CategoryId;

            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}