using BusinessEntities;
using System.Collections.Generic;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderResponseDTO : IdObjectData
    {
        public OrderResponseDTO(Order order) : base(order)
        {
            Id = order.Id;
            CustomerName = order.CustomerName;
            Items = order.Items;
        }

        public string CustomerName { get; set; }
        public decimal Price { get; set; }
        public IReadOnlyCollection<OrderItem> Items { get; set; }
    }
}