using AutoMapper;
using ShoppingCartMicroservice.Dtos;
using ShoppingCartMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ShoppingCartMicroservice.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PostProductDto, Product>().ReverseMap();
            CreateMap<Product, GetProductDto>().ReverseMap();
            CreateMap<ShoppingCart, GetShoppingCartDto>().ReverseMap();
        }
    }
}
