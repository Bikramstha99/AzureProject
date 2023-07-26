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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureProject
{
    public class Delete
    {
        private readonly ILogger _logger;
        public Delete(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Insert>();
        }

        [Function("Delete")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                string idString = req.Query["id"];
                if (string.IsNullOrEmpty(idString) || !int.TryParse(idString, out int id))
                {
                    return new BadRequestObjectResult("Invalid or missing 'id' parameter.");
                }

                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("DELETE FROM Student WHERE Id = @Id", con))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return new OkResult(); // Successful deletion
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
