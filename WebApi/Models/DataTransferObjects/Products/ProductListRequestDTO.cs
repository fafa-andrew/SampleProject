using Data.Extensions;

namespace WebApi.Models.DataTransferObjects.Products
{
    public class ProductListRequestDTO : PagingDTO
    {        
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool InStockOnly { get; set; }
        public ProductSortBy SortBy { get; set; } = ProductSortBy.Name;
        public SortDirection SortDir { get; set; } = SortDirection.Desc;
    }
}