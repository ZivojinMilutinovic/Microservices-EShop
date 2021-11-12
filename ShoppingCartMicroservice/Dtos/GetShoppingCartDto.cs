using ShoppingCartMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Dtos
{
    public class GetShoppingCartDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool Processed { get; set; }

        public ICollection<GetProductDto> Products { get; set; }
    }
}
