using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryMicroservice.Dtos
{
    public class CreateSupplierDto
    {
        
        public string FullName { get; set; }

        public string Country { get; set; }

        public int InventoryId { get; set; }
    }
}
