using AutoMapper;
using ProductMicroservice.Dtos;
using ProductMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ProductMicroservice.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetProductDto>()
                .ReverseMap();
            CreateMap<PostProductDto, Product>()
                .ReverseMap();


        }
    }
}
