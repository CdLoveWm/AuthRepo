using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CookieAuth.Controllers
{
    /// <summary>
    /// 所有需要认证的控制器都继承于该类
    /// </summary>
    [Authorize]
    public abstract class AuthController : Controller
    {
        /// <summary>
        /// 当前登录人信息
        /// </summary>
        public User CurrentUser
        {
            get
            {
                User currentUser = new Auth.Models.User();
                ClaimsPrincipal principal = HttpContext.User;
                currentUser.Email = principal.FindFirstValue(ClaimTypes.Email);
                currentUser.UserCode = principal.FindFirstValue(ClaimTypes.Sid);
                currentUser.UserName = principal.FindFirstValue(ClaimTypes.Name);
                return currentUser;
            }
        }
    }
}
