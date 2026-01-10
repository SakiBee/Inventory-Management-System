namespace IMS.DTOs.Product
{
    /// <summary>
    /// DTO used to Read a product
    /// </summary>
    public class ProductReadDto
    {
        /// <example>1</example>
        public int Id { get; set; }

        /// <example>Laptop</example>
        public string Name { get; set; } = null!;

        /// <example>Electronics</example>
        public string Category { get; set; } = null!;

        /// <example>1200</example>
        public decimal Price { get; set; }

        /// <example>124 </example>
        public int Quantity { get; set; }
    }

}
