using System;
using System.Collections.Generic;
using System.Text;

namespace Crumbs.Persistence.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "new";
        public decimal Total { get; set; }

        public string? UserId { get; set;}
        public UserEntity? User { get; set; }

        public ICollection<OrderLineEntity> OrderLines { get; set; } = new List<OrderLineEntity>();
        public PaymentEntity? Payment { get; set; }
    }
}
