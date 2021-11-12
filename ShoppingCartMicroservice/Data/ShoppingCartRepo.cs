using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShoppingCartMicroservice.Dtos;
using ShoppingCartMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Data
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingCartRepo> _logger;

        public ShoppingCartRepo(AppDbContext context, IMapper mapper,ILogger<ShoppingCartRepo> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<GetShoppingCartDto> AddProductToShoppingCart(PostProductDto postProductDto)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling AddProductToShoppingCart method");
            GetShoppingCartDto result = null;
            var currentShoppingCart= await _context.ShoppingCart.Include(x=>x.Products).FirstOrDefaultAsync(x => x.UserId == postProductDto.UserId && x.Processed == false);
            var product = _mapper.Map<Product>(postProductDto);
            product.NumberOfItems = 1;
            if (currentShoppingCart == null)
            {
                currentShoppingCart = new ShoppingCart()
                {
                    Processed = false,
                    UserId = postProductDto.UserId,
                };
                await _context.ShoppingCart.AddAsync(currentShoppingCart);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"ShoppingCartRepo-->Creating a shopping cart");
            }
            else
            {
                _logger.LogInformation($"ShoppingCartRepo-->Shopping cart already exists");
            }
            
            product.ShoppingCart = currentShoppingCart;
            product.ShoppingCartId = currentShoppingCart.Id;
            var existingProduct = await _context.Product
                .Where(x => x.OriginalProductId == product.OriginalProductId && x.ShoppingCartId==product.ShoppingCartId)
                .FirstOrDefaultAsync();
            if (existingProduct == null)
            {
                _logger.LogInformation($"ShoppingCartRepo-->Adding a new product to the shopping cart");
                _context.Product.Add(product);
            }
            else
            {
                _logger.LogInformation($"ShoppingCartRepo-->Because product already exists in the shopping count, just updating number of items");
                existingProduct.NumberOfItems += 1;
                _context.Product.Update(existingProduct);
            }
            await _context.SaveChangesAsync();
            result = _mapper.Map<GetShoppingCartDto>(currentShoppingCart);
            _logger.LogInformation($"ShoppingCartRepo-->Finishing AddProductToShoppingCart method");
            return result;
        }
        public async Task<GetShoppingCartDto> GetActiveShoppingCartForUser(int userId)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling GetActiveShoppingCartForUser for a user with id ${userId}");
            var cart= await _context.ShoppingCart.Include(x => x.Products).FirstOrDefaultAsync(x => x.UserId == userId && x.Processed == false);
            return _mapper.Map<GetShoppingCartDto>(cart);
        }

        public async Task<IEnumerable<GetShoppingCartDto>> GetAllShoppingCart()
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling GetAllShoppingCart");
            var result= await _context.ShoppingCart.Include(x=>x.Products).ToListAsync();
            return _mapper.Map<IEnumerable<GetShoppingCartDto>>(result);
        }

        public async Task<GetShoppingCartDto> GetShoppingCartById(int id)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling GetShoppingCartById for a shopping cart with id ${id}");
            var result = await _context.ShoppingCart.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);
            return _mapper.Map<GetShoppingCartDto>(result);
        }

        public async Task<bool> ProcessShoppingCart(int shoppingCartId)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling ProcessShoppingCart for a shopping cart with id ${shoppingCartId}");
            var shoppingCart = await _context.ShoppingCart.Include(x=>x.Products).FirstOrDefaultAsync(x => x.Id == shoppingCartId);
            if (shoppingCart == null)
            {
                _logger.LogWarning($"ShoppingCartRepo-->Shopping cart with id ${shoppingCartId} does not exist");
                return false;
            }
            shoppingCart.Processed = true;
            shoppingCart.Products.Clear();
            _context.ShoppingCart.Update(shoppingCart);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"ShoppingCartRepo-->Finishing ProcessShoppingCart for a shopping cart with id ${shoppingCartId}");
            return true;
        }

        public async Task<bool> RemoveProductFromShoppingCart(int shoppingCartId, int productId)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling RemoveProductFromShoppingCart method" +
                $" for a shopping cart with id ${shoppingCartId} and product id ${productId}");
            var shoppingCart = await _context.ShoppingCart.Include(x=>x.Products).FirstOrDefaultAsync(x => x.Id == shoppingCartId);
            if (shoppingCart == null)
            {
                _logger.LogWarning($"ShoppingCartRepo-->Shopping cart with id ${shoppingCartId} does not exist");
                return false;
            }
                
            var product = await _context.Product.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                _logger.LogWarning($"ShoppingCartRepo-->Product with id ${productId} does not exist");
                return false;
            }
                
            shoppingCart.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"ShoppingCartRepo-->Finishing RemoveProductFromShoppingCart method");
            return true;
        }

        public async Task<GetShoppingCartDto>  UpdateShoppingCart(GetShoppingCartDto shoppingCartDto)
        {
            _logger.LogInformation($"ShoppingCartRepo-->Calling  UpdateShoppingCart method");
            var cart =await _context.ShoppingCart.Include(x=>x.Products).FirstOrDefaultAsync(x => x.Id == shoppingCartDto.Id);
            var products = _mapper.Map<ICollection<Product>>(shoppingCartDto.Products);
            cart.Products = products;
            _context.ShoppingCart.Update(cart);
            await _context.SaveChangesAsync();
            var  result = _mapper.Map<GetShoppingCartDto>(cart);
            _logger.LogInformation($"ShoppingCartRepo-->Finishing  UpdateShoppingCart method");
            return result;
        }
    }
}
