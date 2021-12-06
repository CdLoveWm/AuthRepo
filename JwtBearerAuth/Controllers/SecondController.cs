using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerAuth.Controllers
{
    /// <summary>
    /// 基于角色的授权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")] // 1、必须拥有admin角色才能访问
    [Authorize(Roles = "admin,admin1,admin2")] // 2、拥有amdin,admin1,admin2任意一个角色即可访问

    //// 3、同时拥有admin和admin1才能访问
    //[Authorize(Roles = "admin")]
    //[Authorize(Roles = "admin1")]

    public class SecondController : ControllerBase
    {
        [HttpGet, Route("n1")]
        public int Get1() => 1;


        /// <summary>
        /// 指定admin角色才能访问
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "admin")]
        [HttpGet, Route("n2")]
        public int Get2() => 2;
    }
}
