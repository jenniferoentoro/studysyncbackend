using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using user_service.Dtos;
using user_service.model;

namespace user_service.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {

        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

        }
    }
}