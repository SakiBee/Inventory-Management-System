using Microsoft.AspNetCore.Http;
namespace IMS.DTOs.Product
{
    /// <summary>
    /// DTO used to create huge product from csv
    /// </summary>
    public class ProductBulkUploadDto
    {
        /// <example>Upload field for file</example>
        public IFormFile File { get; set; } = null!;
    }
}
