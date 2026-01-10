using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.Product
{
    /// <summary>
    /// DTO used to create a product
    /// </summary>
    public class ProductCreateDTO
    {
        /// <example>Laptop</example>
        [Required(ErrorMessage = "Product name is required!!")]
        public string Name { get; set; } = string.Empty;

        /// <example>1200.0</example>
        [Range(0.01, 1000000000, ErrorMessage = "Price must be between 0.01 and 10000000.")]
        public decimal Price { get; set; } = decimal.Zero;

        /// <example>Electronics</example>
        [Required(ErrorMessage = "Category is required!!")]
        public string Category { get; set; } = null!;

        /// <example>120</example>
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more.")]
        public int Quantity { get; set; } 
    }
}
