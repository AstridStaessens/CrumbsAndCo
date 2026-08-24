using System;
using System.Collections.Generic;
using System.Text;

namespace Crumbs.Persistence.Entities
{
    public class OrderLineEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public int ProductId { get; set; }
        public ProductEntity Product { get; set; }


    }
}
