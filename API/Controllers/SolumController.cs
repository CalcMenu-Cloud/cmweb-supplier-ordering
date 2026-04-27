using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OrderingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class SolumController : Controller
    {

        [HttpPost("GenerateToken")] // Renamed route to avoid conflict
        public async Task<string> GenerateTokenAsync()
        {
            // Create a new HttpClient instance
            using (HttpClient client = new HttpClient())
            {
                // Prepare the JSON payload to send in the body
                var jsonContent = new StringContent("{\"username\": \"rommel.cruz@eg-software.com\", \"password\": \"Calcmenu@esl\"} ", Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("https://eu.common.solumesl.com/common/api/v2/token", jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a string (this might be a token or some other data)
                    string responseBody = await response.Content.ReadAsStringAsync();

                    // Output the response (you can use it as needed)
                    Console.WriteLine("API Response: " + responseBody);
                }
                else
                {
                    // Handle the error if the response is not successful
                    Console.WriteLine("Error: " + response.StatusCode);
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error Details: " + errorResponse);
                }
                return "Generate Token Success";
            }

        }
    }
}
