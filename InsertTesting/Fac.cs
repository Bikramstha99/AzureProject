using Azure.Core;
using Microsoft.AspNetCore.Http;
using System.Text;
namespace xUnitTest
{
    public class HttpRequestFactory
    {
        public HttpRequest GetRequest()
        {
            var jsonpayload = "{ \"Name\": \"John\", \"Place\": \"New York\"}";
            var byteArray = Encoding.UTF8.GetBytes(jsonpayload);
            var requestStream = new MemoryStream(byteArray);
            //var request = new DefaultHttpRequestData(new DefaultHttpContext());
            //request.Body = requestStream;
            //request.Headers.Add("Content-Type", "application/json");
            //return request;

            var context = new DefaultHttpContext();
            context.Request.Body = requestStream;
            return context.Request;
        }
        public HttpRequest GetInvalidRequest()
        {
            var jsonpayload = "{ \"Place\": \"New York\"}"; // Missing 'Name' field
            var byteArray = Encoding.UTF8.GetBytes(jsonpayload);
            var requestStream = new MemoryStream(byteArray);
            //var request = new DefaultHttpRequestData(new DefaultHttpContext());
            //request.Body = requestStream;
            //request.Headers.Add("Content-Type", "application/json");
            //return request;
            var context = new DefaultHttpContext();
            context.Request.Body = requestStream;
            return context.Request;
        }
    }
}