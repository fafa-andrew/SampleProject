using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Products
{
    public class ProductRequestDTO
    {
        private string _name;
        private string _description;

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string Description
        {
            get => _description;
            set => _description = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [Required(ErrorMessage = "Price is required")]
        [RegularExpression(@"^\d*\.?\d*$", ErrorMessage = "Please enter a correct price")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or greter")]
        public int Stock { get; set; } = 0;
    }
}