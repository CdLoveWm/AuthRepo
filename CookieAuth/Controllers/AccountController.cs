using Auth.IServices;
using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CookieAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserServices _userServices;

        public AccountController(IUserServices userServices)
        {
            this._userServices = userServices;
        }

        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ResulModel<string> Login(string userCode, string password)
        {
            // 验证账号密码
            var user = _userServices.FirstOrDefault(it => it.UserCode == userCode && it.Password == password);
            if(user == null)
            {
                return new ResulModel<string>() { Code = 1, Message = "登录失败" };
            }
            // 保存信息
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, user.Password),
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            // 登录，写入Cookie
            HttpContext.SignInAsync(claimsPrincipal).Wait();
            return new ResulModel<string>()
            {
                Code = 0,
                Message = "登录成功"
            };
        }

        /// <summary>
        /// 退出
        /// </summary>
        public IActionResult LogOut()
        {
            // 登出，清除Cookie
            HttpContext.SignOutAsync();
            return RedirectToAction("login");
        }

    }
}
