namespace Crumbs.Domain.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; } = "new";
        public decimal Total { get; set; }
        public string UserId { get; set; } = null!;
        public List<OrderLineModel> OrderLines { get; set; } = [];
    }
}