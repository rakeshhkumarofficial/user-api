using System.Net;

namespace user_api.Models
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; } = true;
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
