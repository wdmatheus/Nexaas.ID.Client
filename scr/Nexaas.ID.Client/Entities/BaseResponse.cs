using System.Net;

namespace Nexaas.ID.Client.Entities
{
    public class BaseResponse<T>
    {
        public T Data { get; set; }

        public HttpStatusCode StatusCode { get; set; }
        
        public string Content { get; set; }
    }
}