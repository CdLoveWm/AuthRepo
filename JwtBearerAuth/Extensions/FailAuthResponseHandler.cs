using Auth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace JwtBearerAuth.Extensions
{
    public class FailAuthResponseHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // 这里一定注意构造函数是ILoggerFactory，不是LoggerFactory，VS自动补全生成的是LoggerFactory，这是错误的
        public FailAuthResponseHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }
        /// <summary>
        /// 自己处理身份验证，这里没实现。是因为认证Schema没用到这个Handler的Schema
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// 没有登录访问时，Token错误访问时，401
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status200OK;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(new ResulModel<string>()
            {
                Code = StatusCodes.Status401Unauthorized,
                Message = "认证失败，请登录"
            }));
        }
        /// <summary>
        /// 成功登录，访问无相应权限的接口时，403
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
        {
            Response.StatusCode = StatusCodes.Status200OK;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(new ResulModel<string>()
            {
                Code = StatusCodes.Status403Forbidden,
                Message = "访问权限不足"
            }));
        }
    }
}
