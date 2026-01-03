using System.ComponentModel.DataAnnotations;

namespace IMS.DTOs.Product
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Product name is required!!")]
        public string Name { get; set; } = null!;

        [Range(0.01, 10000, ErrorMessage = "Price must be between 0.01 and 10000.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Category is required!!")]
        public string Category { get; set; } = null!;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be 0 or more.")]
        public int Quantity { get; set; }
    }
}
