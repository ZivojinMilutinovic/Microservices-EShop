using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Dtos
{
    public class ProductDto
    {
        public string ProductNumber { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public string ClothingType { get; set; }

        public int InventoryId { get; set; }

        public string Event { get; set; }
    }
}
