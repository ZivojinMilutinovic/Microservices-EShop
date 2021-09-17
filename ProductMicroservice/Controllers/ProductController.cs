using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

        public ProductController(IProductRepo productRepo,IMapper mapper)
        {
            _productRepo = productRepo;
            _mapper = mapper;
        }
        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products =await _productRepo.GetAllProducts();
            return Ok(products);
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productRepo.GetProductDetailsById(id);
            if(product is null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/<ProductController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostProductDto postProductDto)
        {
            try
            {
                var entity = await _productRepo.AddProduct(postProductDto);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PostProductDto postProductDto)
        {
            try
            {
                await _productRepo.UpdateProduct(postProductDto, id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest();
            }
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productRepo.DeleteProduct(id);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return NotFound();
            }
        }
    }
}
