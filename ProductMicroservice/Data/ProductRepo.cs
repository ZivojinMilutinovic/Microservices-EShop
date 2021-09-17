using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public ProductRepo(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Product> AddProduct(PostProductDto postProductDto)
        {
            var _object = _mapper.Map<Product>(postProductDto);

            var product = await _context.Products.AddAsync(_object);
            await _context.SaveChangesAsync();
            return product.Entity;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
                _context.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public async Task<IEnumerable<GetProductDto>> GetAllProducts()
        {
            var products= await _context.Products.ToListAsync();
            return _mapper.Map<IEnumerable<GetProductDto>>(products);
        }

        public async Task<GetProductDto> GetProductDetailsById(int productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);
            return _mapper.Map<GetProductDto>(product);
        }

        public async Task<bool> UpdateProduct(PostProductDto postProductDto, int productId)
        {
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
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
