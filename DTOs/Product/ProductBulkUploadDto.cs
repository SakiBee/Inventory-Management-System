using Microsoft.AspNetCore.Http;
namespace IMS.DTOs.Product
{
    public class ProductBulkUploadDto
    {
        public IFormFile File { get; set; } = null!;
    }
}
