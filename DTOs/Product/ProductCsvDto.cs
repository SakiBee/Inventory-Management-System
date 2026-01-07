namespace IMS.DTOs.Product
{
    public class ProductCsvDto
    {
        public string Name { get; set; } = null!;
        public string Category { get; set; } = null!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
