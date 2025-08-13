using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Orders
{
    public class OrderItemDTO
    {
        [Required(ErrorMessage = "Product ID is required")]
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater 0")]
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Unit price is required")]
        [RegularExpression(@"^\d*\.?\d*$", ErrorMessage = "Please enter a correct unit price")]
        public decimal UnitPrice { get; set; }
    }
}