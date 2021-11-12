using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Models
{
    public class Supplier
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Country { get; set; }

        public int InventoryId { get; set; }

        public Inventory Inventory { get; set; }
    }
}
