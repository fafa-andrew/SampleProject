using System;

namespace Core.Services.Orders.Models
{
    public class OrderItemRequest
    {
        public OrderItemRequest(Guid productId, int quantity, decimal unitPrice)
        {
            if (productId == Guid.Empty)
                throw new ArgumentNullException(nameof(productId), "Product ID cannot be empty.");
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
            if (unitPrice < 0)
                throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");

            ProductId = productId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }

        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
