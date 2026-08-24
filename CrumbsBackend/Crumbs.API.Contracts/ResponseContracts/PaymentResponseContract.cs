namespace Crumbs.API.Contracts.ResponseContracts
{
    public class PaymentResponseContract
    {
        public string PaymentUrl { get; set; } = null!;
        public string MolliePaymentId { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}