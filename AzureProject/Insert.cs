using AzureProject.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace AzureProject
{

    public class Insert
    {
        private readonly ILogger _logger;
        public Insert(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Insert>();
            //System.Diagnostics.Debugger.Launch();
        }
        [Function("Insert")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var data = JsonConvert.DeserializeObject<Student>(requestBody);
                var connectionString = "server=LAPTOP-1S4TP60O\\SQLEXPRESS;database=AzureInfo;Trusted_connection=true;Encrypt=False";
                //if (data.Id == null || data.Name == null || data.Place == null)
                //    return new BadRequestResult();
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("insert into Student (Name, Place) values (@Name, @Place)", con))
                    {
                        //command.Parameters.AddWithValue("@Id", data.Id);
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
