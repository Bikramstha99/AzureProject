using AzureProject;
using Microsoft.AspNetCore.Mvc;

namespace FunctionTesting
{
    public class UnitTest1
    {
        [Fact]
        public void Fact1()
        {
            var result = Function1.Run(UnitTestFactory.GetHttpRequest(), null);
            Assert.NotNull(result);

            var expectedvalue = "No query string passed.";
            var actualvalue = ((OkObjectResult)result.Result).Value.ToString();

            Assert.NotNull(actualvalue);
            Assert.Equal(expectedvalue, actualvalue);
                
        }
    }
}