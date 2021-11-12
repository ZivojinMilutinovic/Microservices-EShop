using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductMicroservice.AsyncDataService;
using ProductMicroservice.Data;
using ProductMicroservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProductMicroservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _productRepo;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductRepo productRepo,IMapper mapper
            ,IMessageBusClient messageBusClient,ILogger<ProductController> logger)
        {
            _productRepo = productRepo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
            _logger = logger;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            _logger.LogInformation("ProductController-->GetProducts method called");
            var products =await _productRepo.GetAllProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("ProductController-->Getting a product by id");
            var product = await _productRepo.GetProductDetailsById(id);
            if(product is null)
            {
                _logger.LogWarning("ProductController-->Product was not found");
                return NotFound();
            }
            _logger.LogInformation("ProductController-->Returning a product");
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto postProductDto)
        {
            _logger.LogInformation("ProductController-->Calling Post method");
            try
            {
                var entity = await _productRepo.AddProduct(postProductDto);
                var publishedProduct = _mapper.Map<InventoryPostProductDto>(entity);
                publishedProduct.Event = "ProductCreated";
                _messageBusClient.PublishNewProduct(publishedProduct);
                _logger.LogInformation("ProductController-->Product was successuly created");
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception e)
            {
                _logger.LogError($"ProductController-->An error occured when creating a product ${e.Message}");
                return BadRequest();
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostProductDto postProductDto)
        {
            _logger.LogInformation("ProductController-->Put method called");
            try
            {
                await _productRepo.UpdateProduct(postProductDto, id);
                _logger.LogInformation("ProductController-->Product was updated successfully");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ProductController-->An error occured when updating a product ${e.Message}");
                return BadRequest();
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("ProductController-->Delete method called");
            try
            {
                await _productRepo.DeleteProduct(id);
                _logger.LogInformation("ProductController-->Product was updated successfully");
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"ProductController-->An error occured when deleting a product ${e.Message}");
                return NotFound();
            }
        }
    }
}
