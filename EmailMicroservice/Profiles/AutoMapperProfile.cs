﻿using AutoMapper;
using EmailMicroservice.Dtos;
using EmailMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace EmailMicroservice.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDto, EmailUser>().ReverseMap();
           
        }
    }
}
