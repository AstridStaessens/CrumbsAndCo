using System;
using System.Collections.Generic;
using System.Text;

namespace Crumbs.Persistence.Entities
{
    public class PaymentEntity
    {
        public int Id { get; set; }
        public string MolliePaymentId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Status { get; set; } = "pending";
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }
    }
}
