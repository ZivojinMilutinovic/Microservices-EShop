using ProductMicroservice.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.Dtos
{
    public class GetProductDto
    {
        public string ProductNumber { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public string ClothingType { get; set; }
    }
}
