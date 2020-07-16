using Microsoft.AspNetCore.Mvc;

namespace BookingApi.Data.Util
{
    public abstract class QueryStringParameters
    {
        private const int MaxPageSize = 75;
        [FromQuery(Name = "pageIndex")]
        public int PageNumber { get; set; } = 1;
        [FromQuery(Name = "search")]
        public string SearchString { get; set; }
        [FromQuery(Name = "sort")]
        public string SortString { get; set; }

        private int _pageSize = 50;

        [FromQuery(Name = "pageSize")]
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
