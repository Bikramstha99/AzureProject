using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AzureProject
{
    public static class SumFunction
    {
        [Function("SumFunction")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            try
            {
                // Read the request body as a string
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                // Deserialize the JSON request body to a dynamic object
                dynamic data = JsonConvert.DeserializeObject(requestBody);

                // Get the values of num1 and num2 from the dynamic object
                int num1 = data.num1;
                int num2 = data.num2;

                // Calculate the sum
                int sum = num1 + num2;

                // Return the result as an OkObjectResult
                return new OkObjectResult($"Sum: {sum}");
            }
            catch (Exception ex)
            {
                // If there was an exception, return a BadRequestObjectResult with an error message
                return new BadRequestObjectResult("Invalid input. Please provide valid integers for 'num1' and 'num2'.");
            }
        }
    }
}
