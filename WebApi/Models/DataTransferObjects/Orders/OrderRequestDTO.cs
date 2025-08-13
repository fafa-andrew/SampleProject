using Core.Services.Orders.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderRequestDTO
    {
        private string _customerName;

        [Required(ErrorMessage = "Customer name is required")]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string CustomerName
        {
            get => _customerName;
            set => _customerName = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [Required(ErrorMessage = "At least one item is required")]
        public List<OrderLineItem> Items { get; set; }
    }
}