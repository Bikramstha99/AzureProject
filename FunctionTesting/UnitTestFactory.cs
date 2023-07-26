using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionTesting
{
    public class UnitTestFactory
    {
        public static HttpRequest GetHttpRequest()
        {
            var context= new DefaultHttpContext();
            return context.Request;
        }
    }
}
