
using Microsoft.AspNetCore.Mvc;

using SumFunctionTest;

namespace AzureProject.Tests
{
    public class SumFunctionTests
    {
        [Fact]
        public async Task Run_ValidInput_ReturnsOkResultWithSum()
        {
            // Arrange
            var requestBody = "{\"num1\": 5, \"num2\": 10}";
            var request = UnitTestFactory.GetHttpRequest("POST", requestBody);

            // Act
            var result = await SumFunction.Run(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Sum: 15", okResult.Value);
        }

        [Fact]
        public async Task Run_InvalidInput_ReturnsBadRequest()
        {
            // Arrange
            var requestBody = "{\"num1\": \"invalid\", \"num2\": 10}";
            var request = UnitTestFactory.GetHttpRequest("POST", requestBody);

            // Act
            var result = await SumFunction.Run(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid input. Please provide valid integers for 'num1' and 'num2'.", badRequestResult.Value);
        }
    }
}
