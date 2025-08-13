using BusinessEntities;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserListRequestDTO : PagingDTO
    {
        private string _name;
        private string _email;
        private string _tag;

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