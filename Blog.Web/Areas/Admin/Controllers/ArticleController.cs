using AutoMapper;
using Blog.Entity.DTOs.Articles;
using Blog.Entity.Entities;
using Blog.Service.Services.Abstractions;
using Blog.Web.Consts;
using Blog.Web.ResultMessages;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly IValidator<Article> _validator;
        private readonly IToastNotification _toastNotification;

        public ArticleController(IArticleService articleService, ICategoryService categoryService, IMapper mapper,IValidator<Article> validator, IToastNotification toastNotification)
        {
            this._articleService = articleService;
            this._categoryService = categoryService;
            this._mapper = mapper;
            this._validator = validator;
            this._toastNotification = toastNotification;
        }


        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Superadmin},{RoleConsts.Admin},{RoleConsts.User}")]
        public async Task<IActionResult> Index()
        {

           var articles= await _articleService.GetAllArticlesWithCategoryNonDeletedAsync();

            return View(articles);
        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin} ,{RoleConsts.Admin} ")]
        public async Task<IActionResult> DeletedArticle()
        {

            var articles = await _articleService.GetAllArticlesWithCategoryDeletedAsync();

            return View(articles);
        }



        [HttpGet]
        [Authorize(Roles =$"{RoleConsts.Superadmin} ,{RoleConsts.Admin} ")]
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryService.GetAllCategoriesNonDeteled();
            return View(new ArticleAddDto { Categories= categories} );
        }


        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin},  {RoleConsts.Admin}")]
        public async Task<IActionResult> Add(ArticleAddDto articleAddDto)
        {

            var map =  _mapper.Map<Article>(articleAddDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {
                await _articleService.CreateArticleAsync(articleAddDto);
                _toastNotification.AddSuccessToastMessage(Messages.Article.Add(articleAddDto.Title));
                return RedirectToAction("Index", "Article", new { Area = "Admin" });
            }
            else
            {
                result.AddToModelState(this.ModelState);

            }

            var categories = await _categoryService.GetAllCategoriesNonDeteled();
            return View(new ArticleAddDto { Categories = categories });

        }


        [HttpGet]
        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> Update(Guid articleId)
        {
            var article = await _articleService.GetAllArticlesWithCategoryNonDeletedAsync();
            var categories = await _categoryService.GetAllCategoriesNonDeteled();

            var articleUpdateDto = _mapper.Map<ArticleUpdateDto>(article);
            articleUpdateDto.Categories = categories;

            return View(articleUpdateDto);
        }



        [HttpPost]
        [Authorize(Roles = $"{RoleConsts.Superadmin} ,{RoleConsts.Admin}")]
        public async Task<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
        {

            var map = _mapper.Map<Article>(articleUpdateDto);
            var result = await _validator.ValidateAsync(map);

            if (result.IsValid)
            {

                var title = await _articleService.UpdateArticleAsync(articleUpdateDto);
                _toastNotification.AddSuccessToastMessage(Messages.Article.Update(title), new ToastrOptions { Title="İşlem Başarılı"} );
                return RedirectToAction("Index", "Article", new { Area = "Admin" });

            }
            else
            {
                result.AddToModelState(this.ModelState);

            }
            var categories = await _categoryService.GetAllCategoriesNonDeteled();
            articleUpdateDto.Categories = categories;
            return View(articleUpdateDto);
            //return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }



        [Authorize(Roles = $"{RoleConsts.Superadmin} ,{RoleConsts.Admin}")]
        public async Task<IActionResult>Delete (Guid articleId)
        {
           var title = await _articleService.SafeDeleteArticleAsync(articleId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.Delete(title), new ToastrOptions { Title = "İşlem Başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }



        [Authorize(Roles = $"{RoleConsts.Superadmin}, {RoleConsts.Admin}")]
        public async Task<IActionResult> UndoDelete(Guid articleId)
        {
            var title = await _articleService.UndoDeleteArticleAsync(articleId);
            _toastNotification.AddSuccessToastMessage(Messages.Article.UndoDelete(title), new ToastrOptions { Title = "İşlem Başarılı" });

            return RedirectToAction("Index", "Article", new { Area = "Admin" });
        }
    }
}
