using BusinessEntities;

namespace WebApi.Models.DataTransferObjects.Products
{
    public class ProductResponseDTO : IdObjectData
    {
        public ProductResponseDTO(Product product) : base(product)
        {
            Id = product.Id;
            Name = product.Name;
            Description = product.Description;
            Price = product.Price;
            Stock = product.Stock;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}