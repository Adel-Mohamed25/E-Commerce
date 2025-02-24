using System.Net;

namespace Models.ResponseModels
{
    public class Response<TData> where TData : class
    {
        public Response(HttpStatusCode statusCode = default,
            bool issucceeded = default,
            string message = default,
            string errors = default,
            string meta = default,
            TData? data = default)
        {
            StatusCode = statusCode;
            Message = message;
            Errors = errors;
            Meta = meta;
            Data = data;
            IsSucceeded = issucceeded;
        }

        public HttpStatusCode StatusCode { get; set; }
        public bool IsSucceeded { get; set; }
        public string Message { get; set; }
        public string Errors { get; set; }
        public string Meta { get; set; }
        public TData? Data { get; set; }
    }
}
