using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusinessEntities;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserRequestDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [RegularExpression(@"^[a-zA-Z0-9!@#$%&*()\s?'"":;.,_+={}\[\]\\/-]+$", ErrorMessage = "Enter alphanumeric characters only")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "User type is required")]
        [EnumDataType(typeof(UserTypes), ErrorMessage = "Invalid user type.")]
        public UserTypes Type { get; set; }

        public decimal? AnnualSalary { get; set; }

        public IEnumerable<string> Tags { get; set; }
    }
}