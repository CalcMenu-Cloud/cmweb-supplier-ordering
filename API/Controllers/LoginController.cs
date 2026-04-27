using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private Services.HogashopService _hogashopservice;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _hogashopservice = new Services.HogashopService(_configuration, httpContextAccessor.HttpContext);

        }

        [HttpPost("login")] // Renamed route to avoid conflict
        public ActionResult<object> Login(Models.Credentialbs64 credential)
        {

            Security.LoginSecurity security = new Security.LoginSecurity(_configuration.GetConnectionString("DefaultConnection"));

            Models.UserSession usersession = null;

           if(security.ValidateLogin(credential.credential,ref usersession))
            {
                return Ok(usersession);
            }

            Models.WebResponse errorresponse = new Models.WebResponse();
            errorresponse.status = "error";
            errorresponse.message = "Invalid login";
            return Unauthorized(errorresponse);

          //  return Ok(response); // Return 200 with order data

       
       
        }


    }
}
