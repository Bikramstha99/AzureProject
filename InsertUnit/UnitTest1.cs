using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;
using HttpTriggerProj;
using Microsoft.AspNetCore.Http.Internal;

namespace HttpTriggerProj.Tests
{
    public class InsertTests
    {
        [Fact]
        public async Task Run_ValidRequest_ReturnsOkResult()
        {
            // Arrange
            var requestBody = "{\"Id\": 1, \"Name\": \"John Doe\", \"Place\": \"Somewhere\"}";
            var requestStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));

            var httpRequestData = new DefaultHttpRequest(
                new DefaultHttpContext(),
                HttpMethod.Post,
                new System.Uri("http://localhost"),
                new QueryCollection(),
                new HeaderDictionary(),
                requestStream);
            

           

            // Act
            var result = await function.Run(httpRequestData);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Run_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var requestBody = "invalid json"; // Invalid JSON data
            var requestStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));

            var httpRequestData = new DefaultHttpRequest(
                new DefaultHttpContext(),
                HttpMethod.Post,
                new System.Uri("http://localhost"),
                new QueryCollection(),
                new HeaderDictionary(),
                requestStream);

            var logger = new XunitLogger<Insert>();

            var function = new Insert(logger);

            // Act
            var result = await function.Run(httpRequestData);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal("Unexpected character encountered while parsing value: i. Path '', line 0, position 0.", badRequestResult.Value.ToString());
        }
    }
}
