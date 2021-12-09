using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace JwtBearerAuth.Policy
{
    public class SpecialNameSignalRequirementHandler : AuthorizationHandler<SpecialNameSignalRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SpecialNameSignalRequirement requirement)
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
