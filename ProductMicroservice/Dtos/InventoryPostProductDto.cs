﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductMicroservice.Dtos
{
    public class InventoryPostProductDto
    {
        public int Id { get; set; }

        public string ProductNumber { get; set; }

        public double Price { get; set; }

        public string Description { get; set; }

        public string Collection { get; set; }

        public string ClothingType { get; set; }

        public string Event { get; set; }
    }
}
