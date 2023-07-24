using AzureProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureProject
{
    public class Update
    {
        private readonly ILogger _logger;
        public Update(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Update>();
        }

        [Function("Update")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Student>(requestBody);

                if (data == null || data.Id <= 0)
                {
                    return new BadRequestObjectResult("Invalid or missing student data.");
                }

                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("UPDATE Student SET Name = @Name, Place = @Place WHERE Id = @Id", con))
                    {
                        command.Parameters.AddWithValue("@Id", data.Id);
                        command.Parameters.AddWithValue("@Name", data.Name);
                        command.Parameters.AddWithValue("@Place", data.Place);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new OkResult(); // Successful update
                        }
                        else
                        {
                            return new NotFoundResult(); // No record found with the given ID
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }

}
