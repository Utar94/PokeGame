using Logitar.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PokeGame.Authorization;

internal record RoleAuthorizationRequirement(string Role) : IAuthorizationRequirement;

internal class RoleAuthorizationHandler : AuthorizationHandler<RoleAuthorizationRequirement>
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public RoleAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleAuthorizationRequirement requirement)
  {
    IEnumerable<Claim> claims = context.User.FindAll(Rfc7519ClaimNames.Roles);
    foreach (Claim claim in claims)
    {
      if (claim.Value.Trim().Equals(requirement.Role, StringComparison.InvariantCultureIgnoreCase))
      {
        context.Succeed(requirement);
        break;
      }
    }

    return Task.CompletedTask;
  }
}
