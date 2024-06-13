using Logitar.Portal.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using PokeGame.Extensions;

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
      User? user = httpContext.GetUser();
      if (user != null && user.Roles.Any(role => role.UniqueName.Equals(requirement.Role, StringComparison.InvariantCultureIgnoreCase)))
      {
        context.Succeed(requirement);
      }
    }

    return Task.CompletedTask;
  }
}
