using AzureProject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace xUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async Task InsertEmployee_ValidData_ReturnsOkResult()
        {
            //HttpRequestFactory fac = new HttpRequestFactory();
            //var request = fac.GetRequest();
            var jsonPayload = "{ \"Name\": \"John\", \"Place\": \"New York\"}";
            var byteArray = Encoding.UTF8.GetBytes(jsonPayload);
            var requestStream = new MemoryStream(byteArray);
            var request = new DefaultHttpRequestData(new DefaultHttpContext())
            {
                Body = requestStream,
                Method = "POST",
                Headers = new HeaderDictionary()
                {
                    { "Content-Type", "application/json" }
                }
            };
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var function = new Insert(mockLoggerFactory.Object);
           
            // Act
            var response = await function.Run(request);
            // Assert
            if (response is OkResult)
            {
                Assert.Equal((int)HttpStatusCode.OK, (response as OkResult).StatusCode);
            }
            else
            {
                Assert.True(false, "Expected OkResult but got a different result.");
            }
        }
        [Fact]
        public async Task InsertEmployee_InValidData_ReturnsBadRequestResult()
        {
            HttpRequestFactory fac = new HttpRequestFactory();
            var invalidRequest = fac.GetInvalidRequest();
            // Arrange
            //var mockLoggerFactory = new Mock<ILoggerFactory>();
            var mockLoggerFactory = new Mock<ILoggerFactory>();
            var function = new Insert(mockLoggerFactory.Object);
            // Act
            Debug.WriteLine(function);
            var response = await function.Run(invalidRequest);
            // Assert
            if (response is BadRequestObjectResult)
    {
        var result = response as BadRequestObjectResult;
        Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
        Assert.NotNull(result.Value); // Check for the error message
    }
    else
    {
        Assert.True(false, "Expected BadRequestObjectResult but got a different result.");
    }
        }
    }
}