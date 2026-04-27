using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingAPI.Security
{
    public class SessionSecurity : Controller
    {

        // Inside your controller action method
        public IActionResult SetCookie(string tokenValue)
        {
            // Create a new cookie with name 'token' and value 'tokenValue'
            Response.Cookies.Append("token", tokenValue, new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok("Cookie set successfully");
        }

        // Inside your controller action method
        public IActionResult GetCookie()
        {
            // Retrieve the value of the 'token' cookie
            var tokenValue = Request.Cookies["token"];

            // Use the tokenValue as needed

            // Return a response
            return Ok(tokenValue);
        }

    }
}
