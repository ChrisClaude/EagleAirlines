namespace BookingApi.Data.Util
{
    public abstract class QueryStringParameters
    {
        private const int MaxPageSize = 75;
        public int PageNumber { get; set; } = 1;
        public string SearchString { get; set; }
        public string SortString { get; set; }

        private int _pageSize = 50;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
