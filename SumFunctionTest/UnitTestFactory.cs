using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumFunctionTest
{
    public class UnitTestFactory
    {
      public static HttpRequest GetHttpRequest(string method, string bodyContent)
        {
            var context = new DefaultHttpContext();
            context.Request.Method = method;

            // Set the request body
            var requestBodyStream = new MemoryStream(Encoding.UTF8.GetBytes(bodyContent));
            context.Request.Body = requestBodyStream;

            return context.Request;
        }
    }
}
