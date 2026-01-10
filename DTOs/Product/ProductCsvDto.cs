namespace IMS.DTOs.Product
{
    /// <summary>
    /// DTO used to create products from csv
    /// </summary>
    public class ProductCsvDto
    {
        /// <example>Laptop</example>
        public string Name { get; set; } = null!;
        /// <example>Electronics</example>
        public string Category { get; set; } = null!;
        /// <example>1299.0</example>
        public decimal Price { get; set; }
        /// <example>45</example>
        public int Quantity { get; set; }
    }
}
