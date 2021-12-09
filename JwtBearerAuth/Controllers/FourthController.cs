using Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JwtBearerAuth.Controllers
{
    /// <summary>
    /// 自定义策略的授权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FourthController : ControllerBase
    {
        /// <summary>
        /// 策略和策略处理类合并
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "specialNamePolicy")]
        [HttpGet,Route("1")]
        public int Get1() => 1;

        /// <summary>
        /// 策略和策略处理类分开
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "SpecialNameSignalRequirement")]
        [HttpGet,Route("2")]
        public int Get2() => 2;
    }
}
