using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPizza.Models;

namespace WebApplicationPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StandardPizzaController : ControllerBase
    {
        private readonly StandardPizzaService db;
        public StandardPizzaController(StandardPizzaService context)
        {
            db = context;
        }

        [HttpGet("balance")]
        public async Task<string> CalculateBalance()
        {
            var sum = await db.CalculateBalance();
            return "Balance:" + sum;
        }

        [HttpGet("menu")]
        public async Task<IEnumerable<Pizza>> Menu()
        {
            var pizzas = await db.GetPizzas();
            return pizzas;
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult> Order(string pizzaName)
        {
            try
            {
            var order = await db.CreateOrder(pizzaName);
            return Ok(order);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
