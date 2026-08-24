
using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Models;
using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Mappers.Products
{
    public static class ProductMapper
    {
        public static ProductModel EntityToModel(ProductEntity entity)
        {
            return new ProductModel
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Stock = entity.Stock,
                ImageUrl = entity.ImageUrl,
                IsActive = entity.IsActive,
                CategoryId = entity.CategoryId,
                CategoryName = entity.Category?.Name ?? string.Empty
            };
        }

        public static ProductEntity ModelToEntity(ProductModel model)
        {
            return new ProductEntity
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Stock = model.Stock,
                ImageUrl = model.ImageUrl,
                IsActive = model.IsActive,
                CategoryId = model.CategoryId
            };
        }

        public static ProductModel ContractToDomain(CreateProductRequestContract contract)
        {
            var product = new ProductModel
            {
                Name = contract.Name,
                Description = contract.Description,
                Price = contract.Price,
                Stock = contract.Stock,
                ImageUrl = contract.ImageUrl,
                IsActive = true,
                CategoryId = contract.CategoryId
            };
            //product.Validate();
            return product;
        }

        public static ProductResponseContract ModelToResponseContract(ProductModel product)
        {
            return new ProductResponseContract
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = product.CategoryName
            };
        }
    }
}