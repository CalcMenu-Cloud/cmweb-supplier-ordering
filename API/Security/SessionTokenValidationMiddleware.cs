using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderingAPI.Data;
using Microsoft.Extensions.Configuration;

public class SessionTokenValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;
    private  HttpContext _httpContext;
    public SessionTokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        // Extract session token from request headers
        string sessionToken = context.Request.Headers["Authorization"];
        string requestUrl = context.Request.Path;
        _httpContext = context;

        if (requestUrl.ToLower().Contains("callback") || requestUrl.ToLower().Contains("login") || requestUrl.ToLower().Contains("solum"))
        {
            // Call the next middleware in the pipeline
            await _next(context);
            return;
        }

        // Extract session token from request headers
        string sessionid = "";

        if (context.Request.Headers.ContainsKey("sessionid"))
        {
            sessionid = context.Request.Headers["sessionid"];
        }


        if(string.IsNullOrEmpty(sessionid))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Session ended");
            return;
        }


        // Perform validation logic
        if (!IsValidSessionToken(sessionid))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid session");
            return;
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }

    private bool IsValidSessionToken(string sessionid)
    {
        LoginSessionDataService logsession = new LoginSessionDataService(_configuration.GetConnectionString("DefaultConnection"));
        bool isValidSession = false;
        try
        {
            OrderingAPI.Models.UserSession usersession = new OrderingAPI.Models.UserSession();
           usersession =  logsession.getSNUserSession(sessionid);

          if (usersession == null) return false;

            isValidSession = true;

            _httpContext.Items["InternalUserSession"] = usersession;

            return true;
        }
        catch(Exception ex)
        {
            if (isValidSession) return isValidSession;

            return false;
        }
        
    }
}

