using Logitar.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PokeGame.Authorization;

internal class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public RoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
  {
    HttpContext? httpContext = _httpContextAccessor.HttpContext;
    if (httpContext != null)
    {
      IEnumerable<Claim> roles = httpContext.User.FindAll(Rfc7519ClaimNames.Roles);
      if (roles.Any(claim => claim.Value.Equals(requirement.Role, StringComparison.InvariantCultureIgnoreCase)))
      {
        context.Succeed(requirement);
      }
    }

    return Task.CompletedTask;
  }
}
