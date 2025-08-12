using System;

namespace BusinessEntities
{
    public class OrderItem : IdDateObject
    {
        private Guid _productId;
        private int _quantity;
        private decimal _unitPrice;

        public Guid ProductId => _productId;
        public int Quantity => _quantity;
        public decimal UnitPrice => _unitPrice;
        public decimal LineTotal => UnitPrice * Quantity;

        public void SetProductId(Guid productId) => _productId = productId;
        public void SetQuantity(int quantity) => _quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        public void SetUnitPrice(decimal unitPrice) => _unitPrice = unitPrice >= 0 ? unitPrice : throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
    }
}
