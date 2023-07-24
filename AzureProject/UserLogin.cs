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
    public class UserLogin
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;

        public UserLogin(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<UserLogin>();
            _connectionString = Environment.GetEnvironmentVariable("ConnectionString");
        }

        [Function("UserLogin")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            try
            {
                _logger.LogInformation("C# HTTP trigger function processed a user login request.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var loginRequest = JsonConvert.DeserializeObject<LoginRequest>(requestBody);

                // Validate login request data (e.g., required fields)
                if (string.IsNullOrWhiteSpace(loginRequest.Username) || string.IsNullOrWhiteSpace(loginRequest.Password))
                {
                    return new BadRequestObjectResult("Invalid or missing login request data.");
                }

                // Retrieve user data from the database based on the provided username
                User existingUser;
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT Id, Username, Email, Password FROM Users WHERE Username = @Username", con))
                    {
                        command.Parameters.AddWithValue("@Username", loginRequest.Username);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.Read())
                            {
                                existingUser = new User
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Username = reader.GetString(reader.GetOrdinal("Username")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Password = reader.GetString(reader.GetOrdinal("Password"))
                                };
                            }
                            else
                            {
                                // User not found with the provided username
                                return new NotFoundResult();
                            }
                        }
                    }
                }

                // Hash the provided password and compare it with the stored hashed password
                string hashedPassword = HashPassword(loginRequest.Password);
                if (hashedPassword == existingUser.Password)
                {
                    // Passwords match; login successful
                    // Exclude the password from the response
                    existingUser.Password = null;
                    return new OkObjectResult(existingUser);
                }
                else
                {
                    // Passwords do not match; login failed
                    return new UnauthorizedResult();
                }
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
