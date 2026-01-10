using IMS.DTOs.Product;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    /// <summary>
    /// Manages product-related operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;


        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="dto">Product Read payload</param>
        /// <returns>The Read product</returns>
        /// <response code="201">Product read successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="dto">Product read payload</param>
        /// <returns>The read product</returns>
        /// <response code="201">Product read successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        /// <summary>
        /// Creates a new product
        /// </summary>
        /// <param name="dto">Product creation payload</param>
        /// <returns>The created product</returns>
        /// <response code="201">Product created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO dto)
        {
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update a single product
        /// </summary>
        /// <param name="dto">Product Update payload</param>
        /// <returns>The updated product</returns>
        /// <response code="201">Product updated successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO dto)
        { 
            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="dto">Product delete payload</param>
        /// <returns>The Deleted product</returns>
        /// <response code="201">Product deleted successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles ="Admin, Manager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Filter products by price range
        /// </summary>
        /// <param name="dto">Price filter payload</param>
        /// <returns>Filter product</returns>
        /// <response code="201">Product Filtered successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("priceFilter")]
        public async Task<IActionResult> GetPriceFilter([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var products = await _service.GetProductByPriceRange(minPrice, maxPrice);
            return Ok(products);
        }

        /// <summary>
        /// Creates bulk items of new products from csv
        /// </summary>
        /// <param name="dto">Product creation payload</param>
        /// <returns>The created products</returns>
        /// <response code="201">Products created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("bulk-upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> BulkUpload([FromForm] ProductBulkUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Please upload a valid CSV file.");
            }
            try
            {
                using var stream = dto.File.OpenReadStream();
                int count = await _service.UploadProductFromCsvAsync(stream);
                return Ok(new { message = $"{count} product uploaded successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error processing CSV: {ex.Message}");
            }
        }

        /// <summary>
        /// Product pagination
        /// </summary>
        /// <param name="dto">Pagination payload</param>
        /// <returns>Paginated products</returns>
        /// <response code="201">Product pagination successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}