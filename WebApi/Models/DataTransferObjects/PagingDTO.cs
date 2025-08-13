using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DataTransferObjects
{
    public class PagingDTO
    {
        [Range(0, int.MaxValue, ErrorMessage = "page must be >= 0")]
        public int Page { get; set; } = 1;

        [Range(1, 200, ErrorMessage = "page size must be between 1 and 200")]
        public int PageSize { get; set; } = 10;
    }
}