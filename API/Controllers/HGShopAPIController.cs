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

    public class HGShopAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private Services.HogashopService _hogashopservice;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HGShopAPIController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _hogashopservice = new Services.HogashopService(_configuration, httpContextAccessor.HttpContext);
        }

        [HttpGet("GetDapartment")] // Renamed route to avoid conflict
        public ActionResult<OrderingAPI.Models.Hogashop.Departments> GetDapartment()
        {
            OrderingAPI.Models.Hogashop.Departments dep= _hogashopservice.getDepartments();

            if(!_hogashopservice.isRefreshTokenAvailable)
            {
                _httpContextAccessor.HttpContext.Response.StatusCode = StatusCodes.Status406NotAcceptable;

                return null;
            }

          return  Ok(dep);
            //return new OrderingAPI.Models.HogashopDepartment.Departments();
        }


        [HttpPost("SendOrder")] // Renamed route to avoid conflict
        public async Task<ActionResult<Models.Hogashop.AddProductResult>> SendOrder(Models.SNOrder order)
        {
            if (order == null)
            {
                return BadRequest("An error occurred while processing the request. Order data is null.");
            }

            var result = await _hogashopservice.SendOrderAsync(order);

            if (result == null)
            {
                return StatusCode(500, "Failed to send order."); // Assuming that a failure to send order is an internal server error
            }

            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("GetAccessToken")] // Renamed route to avoid conflict
        public ActionResult GetAccessToken()
        {
            return Ok(_hogashopservice.GetAccessToken());
        }


        [HttpGet("test")] // Renamed route to avoid conflict
        public string test()
        {

            return "Department Test API";
        }


    }
}
