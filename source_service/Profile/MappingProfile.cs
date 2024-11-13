using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using source_service.Dtos.Category;
using source_service.Dtos.Source;
using source_service.Dtos.User;
using source_service.Model;

namespace source_service.Profile
{
    public class MappingProfile : AutoMapper.Profile
    {

        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();
            CreateMap<CreateCategoryDto, Category>();

            CreateMap<Source, SourceDto>();

            CreateMap<SourceDto, Source>();

            CreateMap<CreateSourceDto, Source>();

            CreateMap<User, UserDto>();
        }
    }
}