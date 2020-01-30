using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;

namespace FootballPredictor.Api.Authentication
{
    public class AllowAnonymous : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (IAuthorizationRequirement requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
