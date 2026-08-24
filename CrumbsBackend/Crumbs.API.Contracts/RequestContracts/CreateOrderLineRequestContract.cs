namespace Crumbs.API.Contracts.RequestContracts
{
    public class CreateOrderLineRequestContract
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}