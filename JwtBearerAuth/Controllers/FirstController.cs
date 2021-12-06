using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;

namespace JwtBearerAuth.Controllers
{
    /// <summary>
    /// 简单授权，只要认证成功（登录成功）后即可访问
    /// </summary>
    [Route("api/First")]
    [ApiController]
    [Authorize]
    public class FirstController : ControllerBase
    {
        /// <summary>
        /// 认证通过后才能访问
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("nums1")]
        public int GetNums1() => 1;

        /// <summary>
        /// 添加AllowAnonymous，该方法不需要认证即可访问
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet, Route("nums2")]
        public int GetNums2() => 2;
    }
}
