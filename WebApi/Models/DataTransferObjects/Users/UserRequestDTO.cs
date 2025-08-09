using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserRequestDTO
    {
        private string _name;
        private string _email;

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email
        {
            get => _email;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [Required(ErrorMessage = "User type is required")]
        [EnumDataType(typeof(UserTypes), ErrorMessage = "Invalid user type.")]
        public UserTypes Type { get; set; }

        public int Age { get; set; }

        public decimal? AnnualSalary { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}