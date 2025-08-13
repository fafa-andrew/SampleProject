using System;

namespace BusinessEntities
{
    public class Product : IdDateObject
    {
        private string _name;
        private string _description;
        private decimal _price;
        private int _stock;

        public string Name
        {
            get => _name;
            private set => _name = value;
        }

        public string Description
        {
            get => _description;
            private set => _description = value;
        }

        public decimal Price
        {
            get => _price;
            private set => _price = value;
        }


        public int Stock
        {
            get => _stock;
            private set => _stock = value;
        }


        public void SetName(string name) => _name = name.Trim() ?? throw new ArgumentNullException(nameof(name));

        public void SetDescription(string description) => _description = description.Trim() ?? string.Empty;

        public void SetPrice(decimal price) 
            => _price = price > 0 ? 
            price : 
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");


        public void SetStock(int stock)
            => _stock = stock > 0 ?
            stock :
            throw new ArgumentOutOfRangeException(nameof(stock), "Stock cannot be negative.");

        public void IncreaseStock(int qty)
        {
            if (qty <= 0) throw new ArgumentOutOfRangeException(nameof(qty));
            _stock += qty;
        }

        public void DecreaseStock(int qty)
        {
            if (qty <= 0 || qty > _stock) throw new InvalidOperationException("Insufficient stock.");
            _stock -= qty;
        }

        public bool IsInStock() => _stock > 0;

    }
}
