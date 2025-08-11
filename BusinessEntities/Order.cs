using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessEntities
{
    public class Order : IdDateObject
    {
        private string _customerName;
        private DateTime _orderDate;
        private readonly List<OrderItem> _items = new List<OrderItem>();
        private OrderStatus _status;

        public string CustomerName => _customerName;
        public DateTime OrderDate => _orderDate;
        public OrderStatus Status => _status;
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public decimal TotalAmount => _items.Sum(i => i.LineTotal);

        public void SetCustomerName(string customerName) 
            => _customerName = customerName.Trim() ?? throw new ArgumentNullException(nameof(customerName));
        public void SetOrderDate(DateTime orderDate) 
            => _orderDate = orderDate >= CreatedOn ? orderDate : throw new ArgumentOutOfRangeException(nameof(orderDate), "Order date cannot be before the created date.");
        public void SetStatus(OrderStatus status) => _status = status;

    }
}
