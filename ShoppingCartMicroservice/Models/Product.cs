using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartMicroservice.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public int OriginalProductId { get; set; }

        public string ProductNumber { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public  string ClothingType {get;set;}

        public int ShoppingCartId { get; set; }

        public int NumberOfItems { get; set; }

        public ShoppingCart ShoppingCart { get; set; }

    }

 
}
