using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.DTOs.Categories;
using Blog.Entity.Entities;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Service.Services.Concrete;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IValidator<Category> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification _toastNotification;

        public CategoryController(ICategoryService categoryService, IValidator<Category> validator, IMapper mapper, IToastNotification toastNotification)
        {
            this._categoryService = categoryService;
            this.validator = validator;
            this.mapper = mapper;
            this._toastNotification = toastNotification;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeteled();
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> DeletedCategory()
        {
            var categories = await _categoryService.GetAllCategoriesDeteled();
            return View(categories);
        }

        [HttpGet]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            var map = mapper.Map<Category>(categoryAddDto);
            var result = await validator.ValidateAsync(map);


            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsyn(categoryAddDto);
                _toastNotification.AddSuccessToastMessage(Messages.Category.Add(categoryAddDto.Name), new ToastrOptions { Title = "İşlem Başarılı" });

                return RedirectToAction("Index", "Category", new { Area = ("Admin") });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddWithAjax([FromBody] CategoryAddDto categoryAddDto)
        {
            var map = mapper.Map<Category>(categoryAddDto);
            var result = await validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _categoryService.CreateCategoryAsyn(categoryAddDto);
                _toastNotification.AddSuccessToastMessage(Messages.Category.Add(categoryAddDto.Name), new ToastrOptions { Title = "İşlem Başarılı" });

                return Json(Messages.Category.Add(categoryAddDto.Name));
            }
            else
            {
                _toastNotification.AddErrorToastMessage(result.Errors.First().ErrorMessage, new ToastrOptions { Title = "İşlem Başarısız" });
                return Json(result.Errors.First().ErrorMessage);

            }

        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid categoryId)
        {
            var category = await _categoryService.GetCategoryByGuid(categoryId);
            var map = mapper.Map<Category, CategoryUpdateDto>(category);
            return View(map);
        }
        [HttpPost]
        public async Task<IActionResult> Update(CategoryUpdateDto categoryUpdateDto)
        {

            var map = mapper.Map<Category>(categoryUpdateDto);
            var result = await validator.ValidateAsync(map);


            if (result.IsValid)
            {
                var name = await _categoryService.UpdateCategoryAsyn(categoryUpdateDto);
                _toastNotification.AddSuccessToastMessage(Messages.Article.Update(name), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "Category", new { Area = ("Admin") });
            }
            else
            {
                result.AddToModelState(this.ModelState);
                return View();

            }
        }
        public async Task<IActionResult> Delete(Guid categoryId)
        {
            var name = await _categoryService.SafeDeleteCategoryAsync(categoryId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.Delete(name), new ToastrOptions { Title = "İşlem Başarılı" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }

        public async Task<IActionResult> UndoDelete(Guid categoryId)
        {
            var name = await _categoryService.UndoDeleteCategoryAsync(categoryId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.UndoDelete(name), new ToastrOptions { Title = "İşlem Başarılı" });

            return RedirectToAction("Index", "Category", new { Area = "Admin" });
        }
    }
}
