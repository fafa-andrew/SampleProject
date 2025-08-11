using System;

namespace BusinessEntities
{
    public class OrderItem : IdDateObject
    {
        private string _productName;
        private int _quantity;
        private decimal _unitPrice;

        public string ProductName => _productName;
        public int Quantity => _quantity;
        public decimal UnitPrice => _unitPrice;
        public decimal LineTotal => UnitPrice * Quantity;

        public void SetProductName(string productName) => _productName = productName.Trim() ?? throw new ArgumentNullException(nameof(productName));
        public void SetQuantity(int quantity) => _quantity = quantity > 0 ? quantity : throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        public void SetUnitPrice(decimal unitPrice) => _unitPrice = unitPrice >= 0 ? unitPrice : throw new ArgumentOutOfRangeException(nameof(unitPrice), "Unit price cannot be negative.");
    }
}
