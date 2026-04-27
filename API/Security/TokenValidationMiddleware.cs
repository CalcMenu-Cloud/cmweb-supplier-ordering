using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace OrderingAPI.Security
{

    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenValidationMiddleware> _logger;

        public TokenValidationMiddleware(RequestDelegate next, ILogger<TokenValidationMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["token"];

            if (string.IsNullOrWhiteSpace(token) || !IsValidToken(token))
            {
                _logger.LogInformation("Invalid token or token missing");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token or token missing");
                return;
            }

            await _next(context);
        }

        private bool IsValidToken(string token)
        {
            // Replace this with your actual token validation logic
            return token == "your_secret_token";
        }
    }
}