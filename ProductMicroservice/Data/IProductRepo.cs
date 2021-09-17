using ProductMicroservice.Dtos;
using ProductMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.Data
{
    public interface IProductRepo
    {
        public Task<IEnumerable<GetProductDto>> GetAllProducts();

        public Task<GetProductDto> GetProductDetailsById(int productId);

        public Task<Product> AddProduct(PostProductDto postProductDto);

        public Task<bool> DeleteProduct(int productId);

        public Task<bool> UpdateProduct(PostProductDto postProductDto,int productId);
    }
}
