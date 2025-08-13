using System.Collections.Generic;

namespace WebApi.Models.DataTransferObjects.Users
{
    public class UserListResponseDTO : PagingDTO
    {
        public IEnumerable<UserResponseDTO> Users { get; set; } = new List<UserResponseDTO>();
    }
}