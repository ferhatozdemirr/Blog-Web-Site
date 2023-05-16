using System.Security.Claims;

namespace Blog.Service.Extensions
{
    public static class LoggedInUserExtesions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            return Guid.Parse(principal.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public static string GetLoggedInEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirstValue(ClaimTypes.Email);
        }

        //Bunlara erişebilmen için 
        //private readonly ClaimsPrincipal _user;
        //IHttpContextAccessor httpContextAccessor
        //   _user = httpContextAccessor.HttpContext.User;

        //Tanıtman lazım


    }
}
