namespace MekkysCakes.Shared.DTOs.ProductDTOs
{
    public class ProductQueryParams
    {
        public int? TypeId { get; set; }
        public int? ThemeId { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions Sort { get; set; }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value < 1 ? 1 : value; }
        }

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value < 1 ? DefaultPageSize : (value > MaxPageSize ? MaxPageSize : value); }
        }
    }
}
