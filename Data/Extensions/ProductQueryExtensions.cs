using BusinessEntities;
using System.Linq;

namespace Data.Extensions
{
    public static class ProductQueryExtensions
    {
        public static IQueryable<Product> ApplyFilters(
            this IQueryable<Product> products,
            ProductSortBy sortBy,
            SortDirection sortDir,
            bool inStockOnly,
            decimal? minPrice,
            decimal? maxPrice)
        {
            if (inStockOnly) products = products.Where(p => p.Stock > 0);
            if (minPrice.HasValue) products = products.Where(p => p.Price >= minPrice.Value);
            if (maxPrice.HasValue) products = products.Where(p => p.Price <= maxPrice.Value);

            switch (sortBy)
            {
                case ProductSortBy.Name:
                    products = sortDir == SortDirection.Desc
                        ? products.OrderByDescending(p => p.Name)
                        : products.OrderBy(p => p.Name);
                    break;
                case ProductSortBy.Price:
                    products = sortDir == SortDirection.Desc
                        ? products.OrderByDescending(p => p.Price)
                        : products.OrderBy(p => p.Price);
                    break;
                case ProductSortBy.Stock:
                    products = sortDir == SortDirection.Desc
                        ? products.OrderByDescending(p => p.Stock)
                        : products.OrderBy(p => p.Stock);
                    break;
                default:
                    break;
            }

            return products;
        }
    }
}
