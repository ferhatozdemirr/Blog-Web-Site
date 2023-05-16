using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;

namespace Blog.Service.Services.Abstractions
{
    public interface ICategoryService
    {

        Task<List<CategoryDto>> GetAllCategoriesNonDeteled();
        Task<List<CategoryDto>> GetAllCategoriesDeteled();
        Task<List<CategoryDto>> GetCategoriesNonDeletedTake24();
        Task CreateCategoryAsyn(CategoryAddDto categoryAddDto);
        Task<Category> GetCategoryByGuid(Guid id);
        Task<string> UpdateCategoryAsyn(CategoryUpdateDto categoryUpdateDto);
        Task<string> SafeDeleteCategoryAsync(Guid categoryId);
        Task<string> UndoDeleteCategoryAsync(Guid categoryId);

    }
}
