using BusinessEntities;
using Core.Services;
using Core.Services.Orders.Contracts;
using Core.Services.Orders.Models;
using Core.Services.Products.Contracts;
using Data.Extensions;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.DataTransferObjects.Orders;

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
        private readonly IValidateOrderService _validateOrderService;
        private readonly IUpdateProductService _updateProductService;

        public OrderController(
            ICreateOrderService createOrderService,
            IGetOrderService getOrderService,
            IUpdateOrderService updateOrderService,
            IDeleteOrderService deleteOrderService,
            IValidateOrderService validateOrderService,
            IUpdateProductService updateProductService
            )
        {
            _createOrderService = createOrderService ?? throw new ArgumentNullException(nameof(createOrderService));
            _getOrderService = getOrderService ?? throw new ArgumentNullException(nameof(getOrderService));
            _updateOrderService = updateOrderService ?? throw new ArgumentNullException(nameof(updateOrderService));
            _deleteOrderService = deleteOrderService ?? throw new ArgumentNullException(nameof(deleteOrderService));
            _validateOrderService = validateOrderService ?? throw new ArgumentNullException(nameof(validateOrderService));
            _updateProductService = updateProductService ?? throw new ArgumentNullException(nameof(updateProductService));
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
                    query.MinTotal,
                    query.MaxTotal);

                var orders = await orderQuery
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize).ToListAsync(ct);

                var response = new OrderListResponseDTO
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    Orders = orders.Select(o => new OrderResponseDTO(o)).ToList(),
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
        public async Task<IHttpActionResult> Get(Guid orderId, CancellationToken ct)
        {
            try
            {
                var order = await _getOrderService.GetAsync(orderId, ct);
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

                var validation = await _validateOrderService.ValidateItemsAsync(orderDto.Items, ct);
                if (!validation.Ok) return BadRequestResponse(validation.Errors);

                var order = await _createOrderService.CreateAsync(orderDto.CustomerName, validation.LineItems, ct);
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

                var validation = await _validateOrderService.ValidateItemsAsync(orderDto.Items, ct);
                if (!validation.Ok) return BadRequestResponse(validation.Errors);

                var updatedOrder = await _updateOrderService.UpdateAsync(orderId, orderDto.CustomerName, validation.LineItems, ct);;
                var orderResponse = new OrderResponseDTO(updatedOrder);

                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Update order failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpPatch, Route("{orderId:guid}/update-status")]
        public async Task<IHttpActionResult> UpdateStatus(Guid orderId, OrderStatus status, CancellationToken ct)
        {
            try
            {
                var order = await _getOrderService.GetAsync(orderId, ct);
                if (order == null) return ResourceNotFoundResponse();
                if (order.Status == OrderStatus.Completed) return BadRequest("Cannot updated a completed order");
                if (order.Status == status) return NoContentResponse();

                await _updateOrderService.UpdateStatusAsync(orderId, status, ct);

                var lineItems = order.Items.Select(x => new OrderLineItem(x.ProductId, x.Quantity, x.UnitPrice));

                StockAdjustment adjustment;
                switch (status)
                {
                    case OrderStatus.Cancelled:
                        adjustment = StockAdjustment.Increase;
                        break;
                    case OrderStatus.Completed:
                        adjustment = StockAdjustment.Decrease;
                        break;
                    default:
                        return BadRequest("Invalid order status for stock adjustment");
                }

                await _updateProductService.AdjustStockAsync(lineItems.ToList(), adjustment, ct);

                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error("Update order status failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpDelete, Route("{orderId:guid}/delete")]
        public async Task<IHttpActionResult> Delete(Guid orderId, CancellationToken ct)
        {
            try
            {
                var order = await _getOrderService.GetAsync(orderId, ct);
                if (order == null) return ResourceNotFoundResponse();
                if (order.Status == OrderStatus.Completed) return BadRequest("Cannot delete a completed order");

                await _deleteOrderService.DeleteAsync(orderId, ct);

                var lineItems = order.Items.Select(x => new OrderLineItem(x.ProductId, x.Quantity, x.UnitPrice));
                await _updateProductService.AdjustStockAsync(lineItems.ToList(), StockAdjustment.Increase, ct);

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