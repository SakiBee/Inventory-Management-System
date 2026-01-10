namespace IMS.DTOs.Common
{
    /// <summary>
    /// DTO used for Pagination
    /// </summary>
    public class PagedResultDto<T>
    {
        /// <example>Array of Items</example>
        public IEnumerable<T> Items { get; set; } = [];

        /// <example>120</example>
        public int TotalCount { get; set; }

        /// <example>10</example>
        public int PageNumber { get; set; }

        /// <example>6</example>
        public int PageSize { get; set; }

        /// <example>13</example>
        public int TotalPages => (int) Math.Ceiling(TotalCount / (double)PageSize);
    }
}
