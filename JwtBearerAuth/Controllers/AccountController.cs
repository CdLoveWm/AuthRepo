using Auth.IServices;
using Auth.Models;
using Auth.Models.Jwt;
using IdentityModel;
using JwtBearerAuth.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace JwtBearerAuth.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IUserServices _userServices;

        public AccountController(IOptions<JwtSettings> jwtSettings
            , IUserServices userServices)
        {
            this._jwtSettings = jwtSettings.Value;
            this._userServices = userServices;
        }

        /// <summary>
        /// 登录获取Token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet, Route("token")]
        public ResulModel<string> GetToken([FromQuery] UserDto model)
        {
            User user = _userServices.FirstOrDefault(it => it.UserCode == model.UserCode && it.Password == model.Password);
            if (user == null)
                return new ResulModel<string>() { Code = 1, Message = "用户信息错误" };
            
            // 有效载荷-信息
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(JwtClaimTypes.Audience,_jwtSettings.Audience),
                new Claim(JwtClaimTypes.Issuer,_jwtSettings.Issuer),
                new Claim(JwtClaimTypes.Name, user.UserName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.Id, user.UserCode),
            };
            //string token = JwtHandler.GenerateToken(_jwtSettings, claims);
            string token = JwtRsaHandler.GenerateToken(_jwtSettings, claims);
            return new ResulModel<string>()
            {
                Code = 0,
                Data = token
            };
        }

    }
}
