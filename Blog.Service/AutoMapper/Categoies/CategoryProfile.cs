using AutoMapper;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;

namespace Blog.Service.AutoMapper.Categoies
{
    public class CategoryProfile:Profile
    {
        public CategoryProfile()
        {

            CreateMap<CategoryDto,Category>().ReverseMap();
            CreateMap<CategoryAddDto, Category>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>().ReverseMap();


        }
    }
}

