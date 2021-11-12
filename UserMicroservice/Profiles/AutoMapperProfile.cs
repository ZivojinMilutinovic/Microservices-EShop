using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserMicroservice.Dtos;
using UserMicroservice.Models;

namespace UserMicroservice.Profiles
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDTO, User>().ReverseMap();
            CreateMap<UpdateUserDTO, User>().ReverseMap();
            CreateMap<User, GetUserDTO>().ReverseMap();
            CreateMap<User, UserPublishedDto>().ReverseMap();
        }
    }
}
