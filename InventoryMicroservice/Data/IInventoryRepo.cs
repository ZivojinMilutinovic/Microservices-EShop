using InventoryMicroservice.Dtos;
using InventoryMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Data
{
    public interface IInventoryRepo
    {
        Task<Inventory> GetInventory(int id);
        Task<bool> DeleteInventory(int id);
        Task<bool> UpdateInventory(Inventory invenotry);
        Task<Inventory> CrateInventory(CreateInventoryDto inventoryDto);
        Task<IEnumerable<Inventory>> GetAllInventory();
        Task CreateProductForInventory(ProductDto product);
        Task DeleteProductFromInventory(Product product);
        Task UpdateProductForInventory(Product product);
        Task<Supplier> CreateSupplier(Supplier supplier);
        Task<bool> DeleteSupplier(Supplier supplier);
        Task<bool> UpdateSupplier(Supplier supplier);
    }
}
