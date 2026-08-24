using System;
using System.Collections.Generic;
using System.Text;

namespace Crumbs.Persistence.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; } 
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }
        
        public ICollection<OrderLineEntity> OrderLines { get; set; } = new List<OrderLineEntity>();

    }
}
