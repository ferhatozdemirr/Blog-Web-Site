using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ClaimsPrincipal _user;

        public CategoryService(IUnitofWork unitofWork, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            this.unitofWork = unitofWork;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
            _user = httpContextAccessor.HttpContext.User;

        }

        public async Task<List<CategoryDto>> GetAllCategoriesNonDeteled()
        {
            
            var categories = await unitofWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);

            var map = mapper.Map<List<CategoryDto>>(categories);
            return map;
        }

        public async Task<List<CategoryDto>> GetCategoriesNonDeletedTake24()
        {

            var categories = await unitofWork.GetRepository<Category>().GetAllAsync(x => !x.IsDeleted);

            var map = mapper.Map<List<CategoryDto>>(categories);
            return map.Take(24).ToList();
        }
   

        public async Task CreateCategoryAsyn(CategoryAddDto categoryAddDto)
        {
            var userEmail = _user.GetLoggedInEmail();
            Category category = new(categoryAddDto.Name, userEmail);

            await unitofWork.GetRepository<Category>().AddAsync(category);
            await unitofWork.SaveAsync();
           
        }
        public async Task<Category> GetCategoryByGuid(Guid id)
        {
            var category = await unitofWork.GetRepository<Category>().GetByGuidAsync(id);
            return category;
        }

        public async Task<string> UpdateCategoryAsyn(CategoryUpdateDto categoryUpdateDto)
        {

            var userEmail = _user.GetLoggedInEmail();
            var category = await unitofWork.GetRepository<Category>().GetAsync(x => !x.IsDeleted && x.Id == categoryUpdateDto.Id);

           category.Name= categoryUpdateDto.Name;
            category.ModifiedBy = userEmail;
            category.ModifiedDate = DateTime.Now;




            await unitofWork.GetRepository<Category>().UpdateAsync(category);
            await unitofWork.SaveAsync();

            return category.Name;

        }

        public async Task<string> SafeDeleteCategoryAsync(Guid categoryId)
        {
            var userEmail = _user.GetLoggedInEmail();

            var category = await unitofWork.GetRepository<Category>().GetByGuidAsync(categoryId);

            category.IsDeleted = true;
            category.DeletedDate = DateTime.Now;
            category.DeletedBy = userEmail;

            await unitofWork.GetRepository<Category>().UpdateAsync(category);
            await unitofWork.SaveAsync();
            return category.Name;

        }

        public async Task<List<CategoryDto>> GetAllCategoriesDeteled()
        {
            var categories = await unitofWork.GetRepository<Category>().GetAllAsync(x => x.IsDeleted);

            var map = mapper.Map<List<CategoryDto>>(categories);
            return map;
        }

        public async Task<string> UndoDeleteCategoryAsync(Guid categoryId)
        {

            var category = await unitofWork.GetRepository<Category>().GetByGuidAsync(categoryId);

            category.IsDeleted = false;
            category.DeletedDate = null;
            category.DeletedBy = null;

            await unitofWork.GetRepository<Category>().UpdateAsync(category);
            await unitofWork.SaveAsync();
            return category.Name;
        }

    }
}
