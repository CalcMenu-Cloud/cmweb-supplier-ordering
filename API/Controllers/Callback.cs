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
    public class Callback : ControllerBase
    {


        private readonly IConfiguration _configuration;
        private readonly Data.OrderDataService _databaseService;

        public Callback(IConfiguration configuration)
        {
            _configuration = configuration;
          //  _databaseService = databaseService;
           
        }

        [HttpGet]
        public IActionResult Get(string code, string state, string flag)
        {
            //// Check if either code or state is null or empty
            //if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(state))
            //{
            //    return BadRequest("Both 'code' and 'state' parameters are required.");
            //}



            Security.LoginSecurity loginsecurity = new Security.LoginSecurity(_configuration.GetConnectionString("DefaultConnection"));

            //#region //MAIN CALLBACK URL FOR REDIRECTION to web api or test api platform////////////////
            //if (!string.IsNullOrEmpty(state))
            //{
            //    Models.LoginInfoHG callback =    loginsecurity.getRequestInfo(state);

            //    if(callback==null)
            //    {
            //        return BadRequest($"Received 'code': {code}, 'state': {state} , flag : {flag}");
            //    }

            //    if(string.IsNullOrEmpty(callback.callbackurl))
            //    {
            //        return BadRequest($"Received 'code': {code}, 'state': {state} , flag : {flag}");
            //    }
            //    return Redirect(callback.callbackurl+$"/Callback?code={code}&state={state}&flag=1");
            //}

            //return BadRequest($"Received 'code': {code}, 'state': {state} , flag : {flag}");
            //#endregion
           

            if (!string.IsNullOrEmpty(state))
            {
                //var redirectUrl = $"http://localhost:65386/Callback?code={code}&state={state}&flag=1";
                //return Redirect("http://localhost:4200/login?state=" + state);

               bool isLogin= loginsecurity.HogashopLogin(code,state);
             
                if(!string.IsNullOrEmpty(loginsecurity.baseURL))
                {
                    //return Redirect(loginsecurity.baseURL + "/loginsuccess?state=" + state);


                    return Content("<script>window.location.href = '"+ loginsecurity.baseURL + "/loginsuccess?state=" + state + "';</script>", "text/html");

                }
                else
                {
                    return Redirect("http://localhost:4200/orderapp/loginsuccess?state=" + state);
                }
                
            }
            else
            {
                // You can use the code and state parameters here as needed
               return BadRequest($"Received 'code': {code}, 'state': {state} , flag : {flag}");
            }
        }



        [HttpGet("{status}")]
        public IActionResult Get(string status)
        {
            // You can use the code and state parameters here as needed
            return Ok($"API is UP!");
        }

    }
}
