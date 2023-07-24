using AzureProject.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureProject
{
    public class GetAll
    {
        private readonly ILogger _logger;
        public GetAll(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetAll>();
        }

        [Function("GetAll")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a request to retrieve all data.");

                var connectionString = Environment.GetEnvironmentVariable("ConnectionString");
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Student", con))
                    {
                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            List<Student> students = new List<Student>();

                            while (reader.Read())
                            {
                                Student student = new Student
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Name = reader.GetString(reader.GetOrdinal("Name")),
                                    Place = reader.GetString(reader.GetOrdinal("Place"))
                                };

                                students.Add(student);
                            }

                            return new OkObjectResult(students); // Return the list of students
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
