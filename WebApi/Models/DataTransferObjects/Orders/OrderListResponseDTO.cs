namespace WebApi.Models.DataTransferObjects.Orders
{
    using System.Collections.Generic;

    public class OrderListResponseDTO : PagingDTO
    {
        public IEnumerable<OrderResponseDTO> Orders { get; set; } = new List<OrderResponseDTO>();
    }
}