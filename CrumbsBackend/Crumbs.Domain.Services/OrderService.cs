using Crumbs.API.Contracts.RequestContracts;
using Crumbs.API.Contracts.ResponseContracts;
using Crumbs.Domain.Mappers.Orders;
using Crumbs.Domain.Models.Interfaces;
using Crumbs.Domain.Services.Exceptions;
using Crumbs.Domain.Services.Interfaces;
using Crumbs.Persistence.Entities;

namespace Crumbs.Domain.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Statussen waarin een order door de klant zelf nog geannuleerd mag worden.
        /// Eens een order betaald/in verwerking is, kan dit niet meer via deze weg.
        /// </summary>
        private static readonly string[] CancellableStatuses = { "new", "pending_payment" };

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<OrderResponseContract>> GetAllAsync()
        {
            var entities = await _orderRepository.GetAllAsync();
            return entities
                .Select(e => OrderMapper.EntityToModel(e))
                .Select(OrderMapper.ModelToResponseContract);
        }

        public async Task<OrderResponseContract?> GetByIdAsync(int id)
        {
            var entity = await _orderRepository.GetByIdAsync(id);
            if (entity == null) return null;
            var model = OrderMapper.EntityToModel(entity);
            return OrderMapper.ModelToResponseContract(model);
        }

        public async Task<IEnumerable<OrderResponseContract>> GetByUserIdAsync(string userId)
        {
            var entities = await _orderRepository.GetByUserIdAsync(userId);
            return entities
                .Select(e => OrderMapper.EntityToModel(e))
                .Select(OrderMapper.ModelToResponseContract);
        }

        public async Task<int> CreateAsync(string userId, CreateOrderRequestContract contract)
        {
            if (contract.OrderLines == null || contract.OrderLines.Count == 0)
                throw new BadRequestException("Een bestelling moet minstens 1 product bevatten.");

            var orderLines = new List<OrderLineEntity>();
            var productsToUpdate = new List<ProductEntity>();
            decimal total = 0;

            foreach (var line in contract.OrderLines)
            {
                if (line.Quantity <= 0)
                    throw new BadRequestException("De hoeveelheid van een product moet groter zijn dan 0.");

                var product = await _productRepository.GetByIdAsync(line.ProductId);
                if (product == null || !product.IsActive)
                    throw new BadRequestException($"Product met id {line.ProductId} bestaat niet (meer).");

                if (product.Stock < line.Quantity)
                    throw new BadRequestException(
                        $"Niet genoeg voorraad voor '{product.Name}'. Beschikbaar: {product.Stock}, gevraagd: {line.Quantity}.");

                var unitPrice = product.Price;
                total += unitPrice * line.Quantity;

                orderLines.Add(new OrderLineEntity
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity,
                    UnitPrice = unitPrice
                });

                // Voorraad verlagen; wordt pas opgeslagen nadat de order succesvol is aangemaakt.
                product.Stock -= line.Quantity;
                productsToUpdate.Add(product);
            }

            var order = new OrderEntity
            {
                UserId = userId,
                Date = DateTime.UtcNow,
                Status = "new",
                Total = total,
                OrderLines = orderLines
            };

            var orderId = await _orderRepository.CreateAsync(order);

            foreach (var product in productsToUpdate)
            {
                await _productRepository.UpdateAsync(product);
            }

            return orderId;
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var validStatuses = new[] { "new", "pending_payment", "paid", "in_production", "ready", "completed", "cancelled", "refunded" };
            if (!validStatuses.Contains(status))
                throw new BadRequestException($"Ongeldige status '{status}'. Geldige waarden: {string.Join(", ", validStatuses)}.");

            var entity = await _orderRepository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Order met id {id} werd niet gevonden.");

            entity.Status = status;
            await _orderRepository.UpdateAsync(entity);
        }

        public async Task CancelAsync(string userId, int id)
        {
            var entity = await _orderRepository.GetByIdAsync(id);
            if (entity == null) throw new NotFoundException($"Order met id {id} werd niet gevonden.");

            if (entity.UserId != userId)
                throw new ForbiddenException("Je kan enkel je eigen bestellingen annuleren.");

            if (!CancellableStatuses.Contains(entity.Status))
                throw new BadRequestException(
                    $"Een bestelling met status '{entity.Status}' kan niet meer geannuleerd worden. Neem contact op met de bakkerij.");

            // Voorraad terugzetten voor alle producten in de geannuleerde order.
            foreach (var line in entity.OrderLines)
            {
                var product = await _productRepository.GetByIdAsync(line.ProductId);
                if (product == null) continue;

                product.Stock += line.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            entity.Status = "cancelled";
            await _orderRepository.UpdateAsync(entity);
        }
    }
}
