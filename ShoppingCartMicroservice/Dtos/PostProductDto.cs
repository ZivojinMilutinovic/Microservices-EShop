using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Dtos
{
    public class PostProductDto
    {
        public string ProductNumber { get; set; }

        public int OriginalProductId { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public string ClothingType { get; set; }

        public int NumberOfItems { get; set; }

        public int UserId { get; set; }
    }
}
