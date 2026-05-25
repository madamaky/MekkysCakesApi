namespace MekkysCakes.Shared
{
    public abstract class PaginatedQueryParams
    {
        private int _pageIndex = 1;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = value < 1 ? 1 : value;
        }

        protected virtual int DefaultPageSize => 5;
        protected virtual int MaxPageSize => 10;

        private int? _pageSize;
        public int PageSize
        {
            get => _pageSize ?? DefaultPageSize;
            set => _pageSize = value < 1 ? DefaultPageSize : (value > MaxPageSize ? MaxPageSize : value);
        }
    }
}
