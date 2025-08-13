using System;

namespace BusinessEntities
{
    public class OrderItem : IdDateObject
    {
        private Guid _productId;
        private int _quantity;
        private decimal _unitPrice;

        public Guid ProductId
        {
            get => _productId;
            private set => _productId = value;
        }

        public int Quantity
        {
            get => _quantity;
            private set => _quantity = value;
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            private set => _unitPrice = value;
        }

        public decimal LineTotal => UnitPrice * Quantity;

        public void SetProductId(Guid productId) => _productId = productId;
        public void SetQuantity(int quantity) => _quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        public void SetUnitPrice(decimal unitPrice) => _unitPrice = unitPrice >= 0 ? unitPrice : throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
    }
}
