using AutoMapper;
using InventoryMicroservice.Dtos;
using InventoryMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace InventoryMicroservice.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CreateInventoryDto,Inventory>().ReverseMap();
        }
    }
}
