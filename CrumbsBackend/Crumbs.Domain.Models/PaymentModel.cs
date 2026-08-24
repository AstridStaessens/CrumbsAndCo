namespace Crumbs.Domain.Models
{
    public class PaymentModel
    {
        public int Id { get; set; }
        public string MolliePaymentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime Date { get; set; }
        public int OrderId { get; set; }
    }
}