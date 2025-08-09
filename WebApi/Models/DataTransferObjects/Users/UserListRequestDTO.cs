using BusinessEntities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserListRequestDTO
    {
        private string _name;
        private string _email;
        private string _tag;

        [Range(0, int.MaxValue, ErrorMessage = "skip must be >= 0")]
        public int Skip { get; set; } = 0;

        [Range(1, 200, ErrorMessage = "take must be between 1 and 200")]
        public int Take { get; set; } = 10;

        [EnumDataType(typeof(UserTypes), ErrorMessage = "Invalid user type.")]
        public UserTypes? Type { get; set; }

        public string Tag 
        { 
            get => _tag; 
            set => _tag = string.IsNullOrWhiteSpace(value) ? null : value.Trim(); 
        }


        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email
        {
            get => _email;
            set => _email = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}