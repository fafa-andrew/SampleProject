using BusinessEntities;
using System;
using System.Collections.Generic;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderResponseDTO : IdObjectData
    {
        public OrderResponseDTO(Order order) : base(order)
        {
            Id = order.Id;
            CustomerName = order.CustomerName;
            OrderDate = order.OrderDate;
            Items = order.Items;
        }

        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Price { get; set; }
        public IReadOnlyCollection<OrderItem> Items { get; set; }
    }
}