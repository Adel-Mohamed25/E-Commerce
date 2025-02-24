using System.Net;

namespace Models.ResponseModels
{
    public class PaginationResponse<TData> : Response<TData> where TData : class
    {
        public PaginationResponse(HttpStatusCode statusCode = default,
            bool issucceeded = default,
            string message = default,
            string errors = default,
            string meta = default,
            TData? data = default,
            int totalCount = 0,
            int pageNumber = 1,
            int pageSize = 10) : base(statusCode, issucceeded, message, errors, meta, data)
        {
            TotalCount = totalCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages => (int)Math.Ceiling((decimal)TotalCount / PageSize);

        public bool MoveNext => PageNumber < TotalPages;
        public bool MovePrevious => PageNumber > 1;


    }
}
