using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderingAPI.Services.ThirdPartyAPI
{
    public class HogashopOauth : Controller
    {
   
        public HogashopOauth()
        {
            
        }

        public string GetAccessToken(string accessCode)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("grant_type", "authorization_code");
            data.Add("client_id", "calcmenu");
            data.Add("client_secret", "nnFGBzzQMr4DiDh");
            data.Add("code", accessCode);

            // Create an HttpClient instance
            using var client = new HttpClient();

            // Specify the URL for the token endpoint
            var tokenEndpoint = "https://oauth2.hogashop.ch/access-token";

            // Send the POST request with form data
            using var content = new FormUrlEncodedContent(data);
            var response = client.PostAsync(tokenEndpoint, content).Result;

            TokenModel tokenm = new TokenModel();
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = response.Content.ReadAsStringAsync().Result;
                tokenm = JsonSerializer.Deserialize<TokenModel>(responseContent);
                // Process the response as needed
                Console.WriteLine(responseContent);

                return tokenm.refresh_token;
            }
            else
            {
                tokenm = new TokenModel();
                Console.WriteLine($"Failed to obtain token. Status code: {response.StatusCode}");
                return "";
           
            }

        }

        public TokenModel GetRefreshToken(string refreshToken)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("grant_type", "refresh_token");
            data.Add("client_id", "calcmenu");
            data.Add("client_secret", "nnFGBzzQMr4DiDh");
            data.Add("refresh_token", refreshToken);

            // Create an HttpClient instance
            using var client = new HttpClient();

            // Specify the URL for the token endpoint
            var tokenEndpoint = "https://oauth2.hogashop.ch/access-token";

            // Send the POST request with form data
            using var content = new FormUrlEncodedContent(data);
            var response = client.PostAsync(tokenEndpoint, content).Result;

            TokenModel tokenm = new TokenModel();
            // Check if the response is successful
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                var responseContent = response.Content.ReadAsStringAsync().Result;
                tokenm = JsonSerializer.Deserialize<TokenModel>(responseContent);
                // Process the response as needed
                Console.WriteLine(responseContent);

                return tokenm;
            }
            else
            {
                tokenm = new TokenModel();

                Console.WriteLine($"Failed to obtain token. Status code: {response.StatusCode}");

                return tokenm;

            }

        }

    }


    public class TokenModel
    {
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }

    }

}
