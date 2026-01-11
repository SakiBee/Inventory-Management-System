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
        /// Get all products
        /// </summary>
        /// <returns>List of products</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());


        /// <summary>
        /// Get Product by Id
        /// </summary>
        /// <param name="id">Product identifier</param>
        /// <returns>The requested product</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(typeof(ProductReadDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO dto)
        {
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Update an existing product
        /// </summary>
        /// <param name="id">The Id of the product to update</param>
        /// <param name="dto">Update product data</param>
        /// <returns>No conetent if update succeeds</returns>
        /// <response code="204">Product updated successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO dto)
        { 
            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">The Id of the product to delete</param>
        /// <returns>No content if delete succeeds</returns>
        /// <response code="204">Product deleted successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="404">Product not found</response>
        [Authorize(Roles ="Admin, Manager")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        /// <summary>
        /// Filter products by price range
        /// </summary>
        /// <param name="minPrice">Minimum product price </param>
        /// <param name="maxPrice">Maximum product price </param>
        /// <returns>A list of products within the specified price range</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("price-filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPriceFilter([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var products = await _service.GetProductByPriceRange(minPrice, maxPrice);
            return Ok(products);
        }

        /// <summary>
        /// Bulk upload products from a CSV file
        /// </summary>
        /// <param name="dto">CSV file containing product data</param>
        /// <returns>Number of products successfully uploaded</returns>
        /// <response code="201">Products created successfully</response>
        /// <response code="400">Invalid input</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        /// <response code="500">Server error while processing the CSV file</response>
        [Authorize(Roles = "Admin")]
        [HttpPost("bulk-upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BulkUpload([FromForm] ProductBulkUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("Please upload a valid CSV file.");
            }

            using var stream = dto.File.OpenReadStream();
            int count = await _service.UploadProductFromCsvAsync(stream);

            return Created(
                string.Empty,
                new { message = $"{count} products uploaded successfully." }
            );
        }

        /// <summary>
        /// Get paginated list of products
        /// </summary>
        /// <param name="pageNumber">Page number (starting from 1)</param>
        /// <param name="pageSize">Number of items per page</param>
        /// <returns>Paginated list of products</returns>
        /// <response code="200">Product retrieved successfully</response>
        /// <response code="400">Invalid pagination parameters</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="403">Forbidden</response>
        [HttpGet("pagination")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}