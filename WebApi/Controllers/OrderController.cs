using Core.Services.Orders.Contracts;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Data.Extensions;
using WebApi.Models.DataTransferObjects.Orders;
using Core.Services.Orders.Models;

namespace WebApi.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderController));
        private readonly ICreateOrderService _createOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateOrderService _updateOrderService;
        private readonly IDeleteOrderService _deleteOrderService;

        public OrderController(
            ICreateOrderService createOrderService,
            IGetOrderService getOrderService,
            IUpdateOrderService updateOrderService,
            IDeleteOrderService deleteOrderService
            )
        {
            _createOrderService = createOrderService;
            _getOrderService = getOrderService;
            _updateOrderService = updateOrderService;
            _deleteOrderService = deleteOrderService;
        }

        [HttpGet, Route("list")]
        public async Task<IHttpActionResult> Get([FromUri] OrderListRequestDTO query, CancellationToken ct)
        {
            try
            {
                if (query is null) query = new OrderListRequestDTO();

                if (!ModelState.IsValid) return BadRequestResponse(ModelState);

                var orderQuery = _getOrderService.GetAll().ApplyFilters(
                    query.SortBy,
                    query.SortDir,
                    query.PlacedAfter,
                    query.PlacedBefore,
                    query.MinPrice,
                    query.MaxPrice);

                var orders = await orderQuery
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new OrderResponseDTO(p)).ToListAsync(ct);

                var response = new OrderListResponseDTO
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    Orders = orders
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.Error("Get orders failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpGet, Route("{orderId:guid}", Name = "GetOrderById")]
        public async Task<IHttpActionResult> Get(Guid id, CancellationToken ct)
        {
            try
            {
                var order = await _getOrderService.GetAsync(id, ct);
                if (order == null) return ResourceNotFoundResponse();

                var orderResponse = new OrderResponseDTO(order);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Get order failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpPost, Route("create")]
        public async Task<IHttpActionResult> Create([FromBody] OrderRequestDTO orderDto, CancellationToken ct)
        {
            try
            {
                if (orderDto == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var items = orderDto.Items?.Select(item => new OrderItemRequest(item.ProductId, item.Quantity, item.UnitPrice)).ToList();
                var order = await _createOrderService.CreateAsync(
                    orderDto.CustomerName,
                    orderDto.OrderDate,
                    items,
                    ct
                    );

                var orderResponse = new OrderResponseDTO(order);
                return CreatedAtRoute("GetOrderById", new { orderId = orderResponse.Id }, orderResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while creating order", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpPut, Route("{orderId:guid}/update")]
        public async Task<IHttpActionResult> Update(Guid orderId, [FromBody] OrderRequestDTO orderDto, CancellationToken ct)
        {
            try
            {
                if (orderDto == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var order = await _getOrderService.GetAsync(orderId, ct);
                if (order == null) return ResourceNotFoundResponse();

                var items = orderDto.Items?.Select(item => new OrderItemRequest(item.ProductId, item.Quantity, item.UnitPrice)).ToList();
                var updatedOrder = await _updateOrderService.UpdateAsync(
                     orderId,
                     orderDto.CustomerName,
                     orderDto.OrderDate,
                     items,
                     ct
                     );

                var orderResponse = new OrderResponseDTO(updatedOrder);
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Update order failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpDelete, Route("{orderId:guid}/delete")]
        public async Task<IHttpActionResult> Delete(Guid orderId, CancellationToken ct)
        {
            try
            {
                await _deleteOrderService.DeleteAsync(orderId, ct);
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error("Delete user failed", ex);
                return InternalServerErrorResponse();
            }
        }
    }
}