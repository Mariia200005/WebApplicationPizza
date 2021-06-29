using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationPizza.Models;

namespace WebApplicationPizza.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockOfIngredientsController : ControllerBase
    {
        private readonly StockOfIngredientsService db;
        public StockOfIngredientsController(StockOfIngredientsService context)
        {
            db = context;
        }

        [HttpPost("replenishStockOfIngredients")]
        public async Task<IActionResult> ReplenishStockOfIngredients(string id, int count)
        {
            try
            {
            await db.ReplenishStockOfIngredients(id, count);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { msg = ex.Message });
            }
            return Ok("Success action");
        }
    }
}
