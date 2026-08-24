using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Models;
using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Mappers.Categories
{
    public static class CategoryMapper
    {
        public static CategoryModel EntityToModel(CategoryEntity entity)
        {
            return new CategoryModel
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        public static CategoryEntity ModelToEntity(CategoryModel model)
        {
            return new CategoryEntity
            {
                Id = model.Id,
                Name = model.Name
            };
        }

        public static CategoryModel ContractToDomain(CreateCategoryRequestContract contract)
        {
            return new CategoryModel
            {
                Name = contract.Name
            };
        }

        public static CategoryResponseContract ModelToResponseContract(CategoryModel model)
        {
            return new CategoryResponseContract
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}