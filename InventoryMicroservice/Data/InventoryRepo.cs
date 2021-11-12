using AutoMapper;
using InventoryMicroservice.Dtos;
using InventoryMicroservice.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Data
{
    public class InventoryRepo : IInventoryRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryRepo> _logger;

        public InventoryRepo(AppDbContext context,IMapper mapper,ILogger<InventoryRepo> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<Inventory> CrateInventory(CreateInventoryDto inventoryDto)
        {
            _logger.LogInformation("InventoryRepo-->CreatingInventory method called");
            var _obj = _mapper.Map<Inventory>(inventoryDto);
            var inventory=await _context.Inventory.AddAsync(_obj);
            await _context.SaveChangesAsync();
            _logger.LogInformation("InventoryRepo-->CreatingInventory method finished");
            return inventory.Entity;
        }

        public async Task CreateProductForInventory(ProductDto product)
        {
            _logger.LogInformation("InventoryRepo-->CreatingProductForInvettory  method called");
            try
            {
                var _product = _mapper.Map<Product>(product);
                _product.InventoryId = 1;

                var _obj = await _context.Product.AddAsync(_product);
                await _context.SaveChangesAsync();
                _logger.LogInformation("InventoryRepo-->CreatingProductForInvettory  method finished");
            }catch(Exception e)
            {
                _logger.LogError($"InventoryRepo-->An error occured ${e.Message}");
            }
            
        }

        public Task<Supplier> CreateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteInventory(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductFromInventory(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Inventory>> GetAllInventory()
        {
            throw new NotImplementedException();
        }

        public Task<Inventory> GetInventory(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateInventory(Inventory invenotry)
        {
            throw new NotImplementedException();
        }

        public Task UpdateProductForInventory(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateSupplier(Supplier supplier)
        {
            throw new NotImplementedException();
        }
    }
}
