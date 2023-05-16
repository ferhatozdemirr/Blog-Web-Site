using AutoMapper;
using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using Blog.Service.Extensions;
using Blog.Service.Services.Abstractions;
using Blog.Web.ResultMessages;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Blog.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IValidator<AppUser> validator;
        private readonly IMapper mapper;
        private readonly IToastNotification toastNotification;


        public UserController(IUserService userService,IValidator<AppUser> validator, IMapper mapper, IToastNotification toastNotification)
        {
            this.userService = userService;
            this.validator = validator;
            this.mapper = mapper;
            this.toastNotification = toastNotification;
        }

        public async Task<IActionResult> Index()
        {

          var result=  await userService.GetAllUserWithRoleAsync();

            return View(result);
        }
        [HttpGet]

        public async Task<IActionResult> Add()
        {
            var roles = await userService.GetAllRolesAsync();
            return View(new UserAddDto { Roles =roles  });
        }
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {
            var map = mapper.Map<AppUser>(userAddDto);
            var validation = await validator.ValidateAsync(map);
            var roles = await userService.GetAllRolesAsync();


            if (ModelState.IsValid)
            {
                var result = await userService.CreateUserAsync(userAddDto);

                if (result.Succeeded)
                {
                   

                    toastNotification.AddSuccessToastMessage(Messages.User.Add(userAddDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "User", new { Area = "Admin" });

                }
                else
                {
                    foreach (var errors in result.Errors)
                        ModelState.AddModelError("", errors.Description);

                    validation.AddToModelState(this.ModelState);
                    return View(new UserAddDto { Roles = roles });
                }
            }
            return View(new UserAddDto { Roles = roles });

        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid userId)
        {
            var user = await userService.GetAppUserByIdAsync(userId);
            var roles = await userService.GetAllRolesAsync();
            var map = mapper.Map<UserUpdateDto>(user);
            map.Roles = roles;
            return View(map);

        }

        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            var user = await userService.GetAppUserByIdAsync(userUpdateDto.Id);
            if (user != null)
            {
                var userRole = await userService.GetUserRoleAsync(user);
                var takedRoles = await userService.GetAllRolesAsync();

                if (ModelState.IsValid)
                {
                    var map = mapper.Map(userUpdateDto, user);
                    var validation = await validator.ValidateAsync(map);

                    if (validation.IsValid)
                    {
                        user.UserName = userUpdateDto.Email;
                        user.SecurityStamp = Guid.NewGuid().ToString();
                        var result = await userService.UpdateUserAsync(userUpdateDto);
                        if (result.Succeeded)
                        {
                            toastNotification.AddSuccessToastMessage(Messages.User.Update(userUpdateDto.Email), new ToastrOptions { Title = "İşlem Başarılı" });
                            return RedirectToAction("Index", "User", new { Area = "Admin" });
                        }
                        else
                        {
                            result.AddToIdendtityModelState(this.ModelState);
                            validation.AddToModelState(this.ModelState);
                            return View(new UserAddDto { Roles = takedRoles });
                        }
                    }
                }
            }
            return NotFound();
        }
        public async Task<IActionResult> Delete(Guid userId)
        {
            var result = await userService.DeleteUserAsync(userId);
            if (result.identityResult.Succeeded )
            {
                toastNotification.AddSuccessToastMessage(Messages.User.Delete(result.email), new ToastrOptions { Title = "İşlem Başarılı" });
                return RedirectToAction("Index", "User", new { Area = "Admin" });
            }
            else
            {
                result.identityResult.AddToIdendtityModelState(this.ModelState);
            }

            return NotFound();

        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var profile = await userService.GetUserProfileAsync();

            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileDto userProfileDto)
        {

            if (ModelState.IsValid)
            {
               var result= await userService.UserProfileUpdateAsync(userProfileDto);
                if (result)
                {
                    toastNotification.AddSuccessToastMessage("Profile güncelleme işlemi tamamlandı", new ToastrOptions { Title = "İşlem Başarılı" });
                    return RedirectToAction("Index", "Home", new { Area = "Admin" });

                }
                else
                {
                    var profile= await userService.GetUserProfileAsync();
                    toastNotification.AddErrorToastMessage("Profile güncelleme işlemi tamamlanamadı", new ToastrOptions { Title = "İşlem Başarısız!!!" });
                    return View(profile);
                }
            }else 
                return NotFound();
        }

    }
}
