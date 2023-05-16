using AutoMapper;
using Blog.Data.UnitOfWorks;
using Blog.Entity.DTOs.Users;
using Blog.Entity.Entities;
using Blog.Entity.Enums;
using Blog.Service.Extensions;
using Blog.Service.Helpers.Images;
using Blog.Service.Services.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Service.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUnitofWork unitofWork;
        private readonly IImageHelper ımageHelper;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        RoleManager<AppRole> roleManager;
        private readonly ClaimsPrincipal _user;


        public UserService(IUnitofWork unitofWork, IImageHelper ımageHelper, IHttpContextAccessor httpContextAccessor, IMapper mapper, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            this.unitofWork = unitofWork;
            this.ımageHelper = ımageHelper;
            this.mapper = mapper;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            _user = httpContextAccessor.HttpContext.User;

        }

        public async Task<IdentityResult> CreateUserAsync(UserAddDto userAddDto)
        {
            var map = mapper.Map<AppUser>(userAddDto);
            map.UserName = userAddDto.Email;

            var result = await userManager.CreateAsync(map, string.IsNullOrEmpty(userAddDto.Password) ? "" : userAddDto.Password);

            if (result.Succeeded)
            {
                var findRole = await roleManager.FindByIdAsync(userAddDto.RolId.ToString());
                await userManager.AddToRoleAsync(map, findRole.ToString());
                return result;
            }
            else
            {
                return result;
            }
        }

        public async Task<(IdentityResult identityResult, string? email)> DeleteUserAsync(Guid userId)
        {
            var user = await GetAppUserByIdAsync(userId);
            var result = await userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return (result, user.Email);
            }
            return (result, null);



        }

        public async Task<List<AppRole>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return roles;
        }

        public async Task<List<UserDto>> GetAllUserWithRoleAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var map = mapper.Map<List<UserDto>>(users);

            foreach (var item in map)
            {
                var findUser = await userManager.FindByIdAsync(item.Id.ToString());
                var role = string.Join("", await userManager.GetRolesAsync(findUser));

                item.Role = role;
            }
            return map;
        }

        public async Task<AppUser> GetAppUserByIdAsync(Guid userId)
        {
            return await userManager.FindByIdAsync(userId.ToString());

        }

        public async Task<string> GetUserRoleAsync(AppUser user)
        {
            return string.Join("", await userManager.GetRolesAsync(user));
        }

        public async Task<IdentityResult> UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var user = await GetAppUserByIdAsync(userUpdateDto.Id);
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var userRole = await GetUserRoleAsync(user);
                await userManager.RemoveFromRoleAsync(user, userRole);
                var findRole = await roleManager.FindByIdAsync(userUpdateDto.RolId.ToString());
                await userManager.AddToRoleAsync(user, findRole.Name);
                return result;
            }
            else
            {
                return result;
            }
        }
        public async Task<UserProfileDto> GetUserProfileAsync()
        {
            var userId = _user.GetLoggedInUserId();
            var getUserWithImage = await unitofWork.GetRepository<AppUser>().GetAsync(x => x.Id == userId, x => x.Image);
            var map = mapper.Map<UserProfileDto>(getUserWithImage);
            map.Image.FileName = getUserWithImage.Image.FileName;
            return map;
        }



        private async Task<Guid> UploadImageForUser(UserProfileDto userProfileDto)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);

            var imageUpload = await ımageHelper.Upload($"{userProfileDto.FirstName}{userProfileDto.LastName}", userProfileDto.Photo, ImageType.User);
            Image image = new(imageUpload.FullName, userProfileDto.Photo.ContentType, user.Email);
            await unitofWork.GetRepository<Image>().AddAsync(image);
            return image.Id;
        }




        public async Task<bool> UserProfileUpdateAsync(UserProfileDto userProfileDto)
        {
            var userId = _user.GetLoggedInUserId();
            var user = await GetAppUserByIdAsync(userId);


            var isVeryfied = await userManager.CheckPasswordAsync(user, userProfileDto.CurrentPassword);

            if (isVeryfied && userProfileDto.NewPassword != null )
            {
                var result = await userManager.ChangePasswordAsync(user, userProfileDto.CurrentPassword, userProfileDto.NewPassword);
                if (result.Succeeded)
                {
                    await userManager.UpdateSecurityStampAsync(user);
                    await signInManager.SignOutAsync();
                    await signInManager.PasswordSignInAsync(user, userProfileDto.NewPassword, true, false);

                    mapper.Map(userProfileDto, user);
                    user.ImageId = await UploadImageForUser(userProfileDto);

                    if (userProfileDto.Photo !=null)
                    {
                        user.ImageId = await UploadImageForUser(userProfileDto);
                    }

                    await userManager.UpdateAsync(user);
                    await unitofWork.SaveAsync();

                    return true;
                }
                else return false;
            }
            else if (isVeryfied )
            {
                await userManager.UpdateSecurityStampAsync(user);
                mapper.Map(userProfileDto, user);

                if (userProfileDto.Photo != null)
                {
                    user.ImageId = await UploadImageForUser(userProfileDto);
                }

                await userManager.UpdateAsync(user);
                await unitofWork.SaveAsync();

                await userManager.UpdateAsync(user);
                return true;
            }
            else
            {
                return false;

            }
            }
        }
    }
