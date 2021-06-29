using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPizza.Models;

namespace WebApplicationPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRecipeController : ControllerBase
    {
        private readonly PizzaByUserRecipeService db;

        public UserRecipeController(PizzaByUserRecipeService context)
        {
            db = context;
        }

        [HttpGet("getIngredients")]
        public async Task<ActionResult> GetIngredients()
        {
            try
            {
                var ingredients = await db.GetIngredients();
                return Ok(ingredients);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("createOrder")]
        public async Task<ActionResult> Order(List<string> namesOfIngredients)
        {
            try
            {
                var order = await db.CreateOrder(namesOfIngredients);
                return Ok(order);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
