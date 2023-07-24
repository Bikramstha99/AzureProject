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
    public class UserRegistration
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public UserRegistration(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserRegistration>();
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        }

        [Function("UserRegistration")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a user registration request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var newUser = JsonConvert.DeserializeObject<User>(requestBody);

                // Validate user data (e.g., required fields, unique username, etc.)
                if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.Email) || string.IsNullOrWhiteSpace(newUser.Password))
                {
                    return new BadRequestObjectResult("Invalid or missing user registration data.");
                }

                // Hash the user's password for secure storage
                string hashedPassword = HashPassword(newUser.Password);

                // Save the user data in the database
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)", con))
                    {
                        command.Parameters.AddWithValue("@Username", newUser.Username);
                        command.Parameters.AddWithValue("@Email", newUser.Email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            newUser.Password = null;
                            return new OkObjectResult(newUser); // Successful update
                        }
                        else
                        {
                            return new NotFoundResult(); // No record found with the given ID
                        }

                    }
                }

                // Return the newly created user data (without the password) as part of the response
               
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // Utility method to hash the user's password (replace with a proper hashing library)
        private string HashPassword(string plainPassword)
        {
            // Replace this with a proper password hashing library (e.g., BCrypt, Argon2, etc.)
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainPassword));
        }
    }

}
