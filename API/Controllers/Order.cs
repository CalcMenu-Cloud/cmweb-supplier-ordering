using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OrderingAPI.Models;
using OrderingAPI.Data;
using Microsoft.Extensions.Configuration;

namespace OrderingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]


    public class Order : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IConfiguration _configuration;
        private readonly Data.OrderDataService _databaseService;

        public Order(IConfiguration configuration, Data.OrderDataService databaseService)
        {
            _configuration = configuration;
            _databaseService = databaseService;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }


        [HttpPost("CreateOrder")] // Renamed route to avoid conflict
        public ActionResult<Models.Order> CreateOrder(Models.Order order)
        {

            _databaseService.InsertOrderAndDetails(order);
            // You can perform validation here if needed
            // For demonstration, let's just return the received order
            return Ok(order);
        }


        [HttpPost("CreateOrder2")] // Renamed route to avoid conflict
        public ActionResult<Models.Order> CreateOrder2(Models.Order order)
        {
            // You can perform validation here if needed
            // For demonstration, let's just return the received order
            return Ok(order);
        }


        [HttpGet("GetOrder")] // Renamed route to avoid conflict
        public IActionResult GetOrder(int id)
        {
            bool IsError = false;
            Models.SNOrder order = _databaseService.GetOrderById(id, ref IsError);

            if (IsError)
            {
                return BadRequest(order); // Throw BadRequest with error message
            }

            return Ok(order); // Return 200 with order data
        }

        [HttpGet("GetOrderlist")] // Renamed route to avoid conflict
        public IActionResult GetOrderlist(string ClientId)
        {
            bool IsError = false;
            List<Models.SNOrder> order = _databaseService.GetOrderlistByClientId(ClientId);

            if (order==null)
            {
                return BadRequest("An error occurred while processing the request.");
            }

            return Ok(order);
        }

        [HttpPost("SaveOrder")] // Renamed route to avoid conflict
        public IActionResult SaveOrder(Models.SNOrder order)
        {
            bool IsError = false;

           _databaseService.SaveOrder(order);

            if (order == null)
            {
                return BadRequest("An error occurred while processing the request.");
            }

            return Ok(order);
        }

        [HttpPost("SendOrder")] // Renamed route to avoid conflict
        public IActionResult SendOrder(Models.SNOrder order)
        {
            bool IsError = false;

            _databaseService.SaveOrder(order);

            if (order == null)
            {
                return BadRequest("An error occurred while processing the request.");
            }

            return Ok(order);
        }



    }
}
