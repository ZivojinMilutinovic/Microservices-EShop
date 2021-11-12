using ShoppingCartMicroservice.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Data
{
   public interface IShoppingCartRepo
    {
        Task<IEnumerable<GetShoppingCartDto>> GetAllShoppingCart();

        Task<GetShoppingCartDto> GetShoppingCartById(int id);
        Task<GetShoppingCartDto> AddProductToShoppingCart(PostProductDto postProductDto);

        Task<bool> RemoveProductFromShoppingCart(int shoppingCartId, int productId);

        Task<bool> ProcessShoppingCart(int shoppingCartId);
        Task<GetShoppingCartDto> GetActiveShoppingCartForUser(int userId);

        Task<GetShoppingCartDto> UpdateShoppingCart(GetShoppingCartDto shoppingCartDto);
    }
}
