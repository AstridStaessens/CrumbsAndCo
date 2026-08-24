namespace Crumbs.API.Contracts.RequestContracts
{
    public class CreateOrderRequestContract
    {
        public List<CreateOrderLineRequestContract> OrderLines { get; set; } = [];
    }
}