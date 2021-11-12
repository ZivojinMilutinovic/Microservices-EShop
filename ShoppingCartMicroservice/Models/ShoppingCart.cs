using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public bool Processed { get; set; }

        public ICollection<Product> Products { get; set; }

    }
}
