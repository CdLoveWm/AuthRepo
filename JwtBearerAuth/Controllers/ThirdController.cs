using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Xml.Linq;

namespace JwtBearerAuth.Controllers
{
    /// <summary>
    /// 基于声明的授权
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Policy = "RequireEmpName")]
    //[Authorize(Policy = "SpecialEmpName")]
    public class ThirdController : ControllerBase
    {

        [HttpGet, Route("n1")]
        public int Get1() => 1;
    }
}
