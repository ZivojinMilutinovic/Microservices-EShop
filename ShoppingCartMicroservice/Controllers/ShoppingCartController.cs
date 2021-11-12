using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCartMicroservice.Data;
using ShoppingCartMicroservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingCartMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ILogger<ShoppingCartController> _logger;
        private readonly IShoppingCartRepo _repo;

        public ShoppingCartController(IShoppingCartRepo repo,ILogger<ShoppingCartController> logger)
        {
            _logger = logger;
            _repo=repo;
        }
        // GET: api/<ShoppingCartController>
        [HttpGet]
        public async Task<IActionResult> GetAllShoppingCarts()
        {
            _logger.LogInformation("ShoppingCartController-->Calling method for getting all shopping carts");
            var result = await _repo.GetAllShoppingCart();
            return Ok(result);
        }

        // GET api/<ShoppingCartController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("ShoppingCartController-->Calling method for getting shopping cart by id ");
            var result = await _repo.GetShoppingCartById(id);
            if (result != null)
            {
                _logger.LogInformation("ShoppingCartController-->Shopping cart was found");
                return Ok(result);
            }
            _logger.LogInformation($"ShoppingCartController-->Shopping cart with id ${id} was found ");
            return NotFound();

        }
        [HttpGet("active/{userId}")]
        public async Task<IActionResult> GetActiveShoppingCard(int userId)
        {
            _logger.LogInformation("ShoppingCartController-->Calling method for getting an active shopping cart");
            var result = await _repo.GetActiveShoppingCartForUser(userId);
            if (result != null)
            {
                _logger.LogInformation("ShoppingCartController-->Active Shopping cart was found");
                return Ok(result);
            }
            _logger.LogInformation($"ShoppingCartController-->Active Shopping cart with for user with id ${userId} was found ");
            return NotFound();

        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto postProductDto)
        {
            _logger.LogInformation($"ShoppingCartController-->Calling method for creating a shopping cart");
            var result =await  _repo.AddProductToShoppingCart(postProductDto);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        // PUT api/<ShoppingCartController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id)
        {
            _logger.LogInformation($"ShoppingCartController-->Calling method for processing a shopping cart");
            var result =await _repo.ProcessShoppingCart(id);
            if (result)
            {
                _logger.LogInformation($"ShoppingCartController-->Shopping cart was successfully processed");
                return Ok();
            }
            _logger.LogInformation($"ShoppingCartController-->Bad request when proccesing a shopping cart");
            return BadRequest();
        }

        [HttpPut("shoppingCart/{id}")]
        public async Task<IActionResult> UpdateShoppingCart(int id, [FromBody]GetShoppingCartDto shoppingCartDto)
        {
            _logger.LogInformation($"ShoppingCartController-->Calling method for updating a shopping cart");
            try
            {
                var result = await _repo.UpdateShoppingCart(shoppingCartDto);
                _logger.LogInformation($"ShoppingCartController-->Shopping cart was successfully updated");
                return Ok(result);
            }
            catch (Exception)
            {
                _logger.LogWarning($"ShoppingCartController-->Bad request when updating a shopping cart");
                return BadRequest();
            }
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{shoppingCartId}/{productId}")]
        public async Task<IActionResult> Delete(int shoppingCartId,int productId)
        {
            _logger.LogInformation($"ShoppingCartController-->Calling method for deleting a shopping cart");
            var result = await _repo.RemoveProductFromShoppingCart(shoppingCartId,productId);
            if (result)
            {
                _logger.LogInformation($"ShoppingCartController-->Calling method for deleting a shopping cart");
                return Ok();
            }
            _logger.LogWarning($"ShoppingCartController-->Bad request when deleting a shopping cart");
            return BadRequest();
        }
    }
}
