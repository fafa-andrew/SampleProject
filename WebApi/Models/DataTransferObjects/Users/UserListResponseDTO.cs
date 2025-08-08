using System.Collections.Generic;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserListResponseDTO
    {
        public IEnumerable<UserResponseDTO> Users { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; }
    }
}