using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Models
{
    public class Inventory
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        public ICollection<Supplier> Supplier { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
