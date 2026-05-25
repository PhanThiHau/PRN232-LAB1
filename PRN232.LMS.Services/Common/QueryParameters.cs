namespace PRN232.LMS.Services.Common
{
    public class QueryParameters
    {
        private int _page = 1;
        private int _size = 10;
        private const int MaxPageSize = 50;

        public string? Search { get; set; }
        public string? Sort { get; set; }

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int Size
        {
            get => _size;
            set => _size = value > MaxPageSize ? MaxPageSize : (value < 1 ? 1 : value);
        }

        public string? Fields { get; set; }
        public string? Expand { get; set; }
    }
}
