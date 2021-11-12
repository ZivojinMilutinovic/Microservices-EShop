using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Dtos
{
    public class GetProductDto
    {
        public int OriginalProductId { get; set; }

        public string ProductNumber { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public int NumberOfItems { get; set; }

        public string ClothingType { get; set; }

    }
}
