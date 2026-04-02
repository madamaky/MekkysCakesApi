namespace MekkysCakes.Shared.DTOs.OrderDTOs
{
    public class OrderQueryParams
    {
        public string? Email { get; set; }
        public Guid? OrderId { get; set; } = null;

        public OrderStatusDTO? OrderStatus { get; set; }
        public OrderSortingOptions Sort { get; set; }

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
