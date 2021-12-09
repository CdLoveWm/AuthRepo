using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerAuth.Policy
{
    /// <summary>
    /// 特殊Name判断
    /// 只有张三才能通过鉴权
    /// </summary>
    public class SpecialNameRequirement : AuthorizationHandler<SpecialNameRequirement>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SpecialNameRequirement requirement)
        {
            if (context.User != null)
            {
                bool fond = context.User.Claims.Any(it => it.Type == JwtClaimTypes.Name && it.Value == "张三");
                if (fond) context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }
}
