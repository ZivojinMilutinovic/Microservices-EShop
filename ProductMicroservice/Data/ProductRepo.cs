using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductMicroservice.Dtos;
using ProductMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.Data
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductRepo> _logger;

        public ProductRepo(AppDbContext context,IMapper mapper,ILogger<ProductRepo> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Product> AddProduct(PostProductDto postProductDto)
        {
            _logger.LogInformation("ProductRepo-->AddProduct method called");
            var _object = _mapper.Map<Product>(postProductDto);
            var product = await _context.Products.AddAsync(_object);
            await _context.SaveChangesAsync();
            _logger.LogInformation("ProductRepo-->Product was saved successfully");
            return product.Entity;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                _logger.LogInformation("ProductRepo-->DeleteProduct method called");
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                _context.Remove(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ProductRepo-->Changes were saved to database");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ProductRepo-->An error occured in DeleteProduct method ${e.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<GetProductDto>> GetAllProducts()
        {
            var products= await _context.Products.ToListAsync();
            _logger.LogInformation("ProductRepo-->Returning list of products");
            return _mapper.Map<IEnumerable<GetProductDto>>(products);
        }

        public async Task<GetProductDto> GetProductDetailsById(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            _logger.LogInformation("ProductRepo-->Returning product by id");
            return _mapper.Map<GetProductDto>(product);
        }

        public async Task<bool> UpdateProduct(PostProductDto postProductDto, int productId)
        {
            _logger.LogInformation("ProductRepo-->Starting Update product method");
            try
            {
                var _object = _mapper.Map<Product>(postProductDto);
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                product.ProductNumber = _object.ProductNumber;
                product.Price = _object.Price;
                product.Collection = _object.Collection;
                product.ClothingType = _object.ClothingType;
                product.Description = _object.Description;
                _context.Update(product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("ProductRepo-->Finishing Update product method");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"ProductRepo-->An error occured in UpdateProduct method ${e.Message}");
                return false;
            }
        }
    }
}
