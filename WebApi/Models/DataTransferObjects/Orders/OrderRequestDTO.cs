using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderRequestDTO
    {
        private string _customerName;
        private DateTime _orderDate;

        [Required(ErrorMessage = "Customer name is required")]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string CustomerName
        {
            get => _customerName;
            set => _customerName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [Required(ErrorMessage = "Order date is required")]
        public DateTime OrderDate
        {
            get => _orderDate;
            private set => _orderDate = value;
        }

        public IEnumerable<OrderItemDTO> Items { get; set; }
    }
}