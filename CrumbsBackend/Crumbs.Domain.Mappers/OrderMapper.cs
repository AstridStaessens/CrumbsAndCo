using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Models;
using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Mappers.Orders
{
    public static class OrderMapper
    {
        public static OrderModel EntityToModel(OrderEntity entity)
        {
            return new OrderModel
            {
                Id = entity.Id,
                Date = entity.Date,
                Status = entity.Status,
                Total = entity.Total,
                UserId = entity.UserId ?? string.Empty,
                OrderLines = entity.OrderLines
                    .Select(ol => new OrderLineModel
                    {
                        Id = ol.Id,
                        Quantity = ol.Quantity,
                        UnitPrice = ol.UnitPrice,
                        OrderId = ol.OrderId,
                        ProductId = ol.ProductId,
                        ProductName = ol.Product?.Name ?? string.Empty
                    }).ToList()
            };
        }

        public static OrderResponseContract ModelToResponseContract(OrderModel model)
        {
            return new OrderResponseContract
            {
                Id = model.Id,
                Date = model.Date,
                Status = model.Status,
                Total = model.Total,
                UserId = model.UserId,
                OrderLines = model.OrderLines.Select(ol => new OrderLineResponseContract
                {
                    Id = ol.Id,
                    Quantity = ol.Quantity,
                    UnitPrice = ol.UnitPrice,
                    ProductId = ol.ProductId,
                    ProductName = ol.ProductName
                }).ToList()
            };
        }
    }
}