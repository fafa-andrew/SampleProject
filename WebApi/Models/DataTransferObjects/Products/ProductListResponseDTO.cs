namespace WebApi.Models.DataTransferObjects.Products
{
    using System.Collections.Generic;

    public class ProductListResponseDTO : PagingDTO
    {
        public IEnumerable<ProductResponseDTO> Products { get; set; } = new List<ProductResponseDTO>();
    }
}