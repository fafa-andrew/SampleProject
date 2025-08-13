using Data;
using System;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderListRequestDTO : PagingDTO
    {
        public DateTime? PlacedAfter { get; set; }
        public DateTime? PlacedBefore { get; set; }
        public decimal? MinTotal { get; set; }
        public decimal? MaxTotal { get; set; }
        public OrderSortBy SortBy { get; set; } = OrderSortBy.OrderDate;
        public SortDirection SortDir { get; set; } = SortDirection.Desc;
    }
}