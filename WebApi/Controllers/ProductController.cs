using Core.Services.Products.Contracts;
using Data.Extensions;
using log4net;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.Models.DataTransferObjects.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ProductController));
        private readonly ICreateProductService _createProductService;
        private readonly IGetProductService _getProductService;
        private readonly IUpdateProductService _updateProductService;
        private readonly IDeleteProductService _deleteProductService;

        public ProductController(
            ICreateProductService createProductService,
            IGetProductService getProductService,
            IUpdateProductService updateProductService,
            IDeleteProductService deleteProductService
            )
        {
            _createProductService = createProductService;
            _getProductService = getProductService;
            _updateProductService = updateProductService;
            _deleteProductService = deleteProductService;
        }

        [HttpGet, Route("list")]
        public async Task<IHttpActionResult> Get([FromUri] ProductListRequestDTO query, CancellationToken ct)
        {
            try
            {
                if(query is null) query = new ProductListRequestDTO();

                if (!ModelState.IsValid) return BadRequestResponse(ModelState);

                var productQuery = _getProductService.GetAll().ApplyFilters(
                    query.SortBy, 
                    query.SortDir, 
                    query.InStockOnly, 
                    query.MinPrice, 
                    query.MaxPrice);
                
                var products = await productQuery
                    .Skip((query.Page - 1) * query.PageSize)
                    .Take(query.PageSize)
                    .Select(p => new ProductResponseDTO(p)).ToListAsync(ct);

                var response = new ProductListResponseDTO
                {
                    Page = query.Page,
                    PageSize = query.PageSize,
                    Products = products
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                _logger.Error("Get products failed", ex);
                return InternalServerErrorResponse();
            }        
        }

        [HttpGet, Route("{productId:guid}", Name = "GetProductById")]
        public async Task<IHttpActionResult> Get(Guid id, CancellationToken ct)
        {
            try
            {
                var product = await _getProductService.GetAsync(id, ct);
                if (product == null) return ResourceNotFoundResponse();

                var productResponse = new ProductResponseDTO(product);
                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Get product failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpPost, Route("create")]
        public async Task<IHttpActionResult> Create([FromBody] ProductRequestDTO productDto, CancellationToken ct)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var product = await _createProductService.CreateAsync(
                    productDto.Name,
                    productDto.Description,
                    productDto.Price,
                    productDto.Stock,
                    ct
                    );

                var productResponse = new ProductResponseDTO(product);
                return CreatedAtRoute("GetProductById", new { productId = productResponse.Id }, productResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Error while creating product", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpPut, Route("{productId:guid}/update")]
        public async Task<IHttpActionResult> Update(Guid productId, [FromBody] ProductRequestDTO productDto, CancellationToken ct)
        {
            try
            {
                if (productDto == null || !ModelState.IsValid) return BadRequestResponse(ModelState);

                var product = await _getProductService.GetAsync(productId, ct);
                if (product == null) return ResourceNotFoundResponse();

                var updatedProduct = await _updateProductService.UpdateAsync(
                     productId,
                     productDto.Name,
                     productDto.Description,
                     productDto.Price,
                     productDto.Stock,
                     ct
                     );

                var productResponse = new ProductResponseDTO(updatedProduct);
                return Ok(productResponse);
            }
            catch (Exception ex)
            {
                _logger.Error("Update product failed", ex);
                return InternalServerErrorResponse();
            }
        }

        [HttpDelete, Route("{productId:guid}/delete")]
        public async Task<IHttpActionResult> Delete(Guid productId, CancellationToken ct)
        {
            try
            {
                await _deleteProductService.DeleteAsync(productId, ct);
                return NoContentResponse();
            }
            catch (Exception ex)
            {
                _logger.Error("Delete product failed", ex);
                return InternalServerErrorResponse();
            }
        }
    }
}