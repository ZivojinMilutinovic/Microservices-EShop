using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShippingMicroservice.Data;
using ShippingMicroservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShippingMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShippingRepo _repo;
        private readonly ILogger<ShippingController> _logger;

        public ShippingController(IShippingRepo repo,ILogger<ShippingController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        // GET: api/<ShippingController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ShippingController>/5
        [HttpGet("{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            _logger.LogInformation("ShippingController-->Calling Get method for shipping details");
            var result = await _repo.GetShippingDetailsForUser(userId);
            if (result == null)
            {
                _logger.LogWarning($"ShippingController-->ShippingDetails for user with given id ${userId}  were not found");
                return NotFound();
            }
            _logger.LogInformation("ShippingController-->Shipping details were found");
            return Ok(result);
        }

        // POST api/<ShippingController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateShippingDetailsDto dto)
        {
            _logger.LogInformation("ShippingController-->Calling Post method for shipping details");
            var result = await _repo.CreateShippingDetails(dto);
            return Ok(result);
        }

        // PUT api/<ShippingController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ShippingController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
