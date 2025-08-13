using System;
using System.Linq;
using BusinessEntities;

namespace Data.Extensions
{
    public static class OrderQueryExtensions
    {
        public static IQueryable<Order> ApplyFilters(
            this IQueryable<Order> orders,
            OrderSortBy sortBy,
            SortDirection sortDir,
            DateTime? placedAfter,
            DateTime? placedBefore,
            decimal? minTotal,
            decimal? maxTotal)
        {
            if (placedAfter.HasValue) orders = orders.Where(o => o.CreatedOn >= placedAfter.Value);
            if (placedBefore.HasValue) orders = orders.Where(o => o.CreatedOn <= placedBefore.Value);
            if (minTotal.HasValue) orders = orders.Where(o => o.TotalAmount >= minTotal.Value);
            if (maxTotal.HasValue) orders = orders.Where(o => o.TotalAmount <= maxTotal.Value);

            switch (sortBy)
            {
                case OrderSortBy.TotalAmount:
                    orders = (sortDir == SortDirection.Desc)
                        ? orders.OrderByDescending(o => o.TotalAmount)
                        : orders.OrderBy(o => o.TotalAmount);
                    break;

                case OrderSortBy.OrderDate:
                default:
                    orders = (sortDir == SortDirection.Desc)
                        ? orders.OrderByDescending(o => o.CreatedOn)
                        : orders.OrderBy(o => o.CreatedOn);
                    break;
            }

            return orders;
        }
    }
}
