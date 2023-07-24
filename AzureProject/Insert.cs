using AzureProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Net;
namespace HttpTriggerProj
{
    public class Insert
    {
        private readonly ILogger _logger;
        public Insert(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Insert>();
        }
        [Function("Insert")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Student>(requestBody);
                var response = req.CreateResponse(HttpStatusCode.OK);
                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("insert into Student (Id,Name, Place) values (@Id,@Name, @Place)", con))
                    {
                        command.Parameters.AddWithValue("@Id", data.Id);
                        command.Parameters.AddWithValue("@Name", data.Name);
                        command.Parameters.AddWithValue("@Place", data.Place);
                        int x = command.ExecuteNonQuery();
                    }
                }
                return new OkResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}