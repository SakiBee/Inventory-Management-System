using IMS.DTOs.Product;
using IMS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductsController(IProductService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDTO dto)
        {
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [Authorize(Roles = "Admin, Manager")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDTO dto)
        {
            return await _service.UpdateAsync(id, dto) ? NoContent() : NotFound();
        }

        [Authorize(Roles ="Admin, Manager")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await _service.DeleteAsync(id) ? NoContent() : NotFound();
        }

        [HttpGet("priceFilter")]
        public async Task<IActionResult> GetPriceFilter([FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var products = await _service.GetProductByPriceRange(minPrice, maxPrice);
            return Ok(products);
        }

        //[Authorize(Roles = "Admin")]
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

        [HttpGet("pagination")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _service.GetPagedAsync(pageNumber, pageSize);
            return Ok(result);
        }
    }
}