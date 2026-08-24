namespace Crumbs.API.Contracts.ResponseContracts
{
    public class OrderResponseContract
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = null!;
        public decimal Total { get; set; }
        public string UserId { get; set; } = null!;
        public List<OrderLineResponseContract> OrderLines { get; set; } = [];
    }
}