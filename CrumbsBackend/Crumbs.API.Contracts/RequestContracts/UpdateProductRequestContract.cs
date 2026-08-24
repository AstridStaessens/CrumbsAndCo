namespace Crumbs.API.Contracts.RequestContracts
{
    public class UpdateProductRequestContract
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}