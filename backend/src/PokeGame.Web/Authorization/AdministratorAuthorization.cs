using Logitar.Portal.Core.Users.Models;
using Microsoft.AspNetCore.Authorization;

namespace PokeGame.Web.Authorization
{
  internal class AdministratorAuthorizationRequirement : IAuthorizationRequirement
  {
  }

  internal class AdministratorAuthorizationHandler : AuthorizationHandler<AdministratorAuthorizationRequirement>
  {
    private readonly HashSet<string> _administrators;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AdministratorAuthorizationHandler(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
    {
      _administrators = configuration.GetSection("Administrators").Get<IEnumerable<string>>()
        .Select(x => x.ToUpper())
        .ToHashSet();

      _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdministratorAuthorizationRequirement requirement)
    {
      if (_httpContextAccessor.HttpContext != null)
      {
        UserModel? user = _httpContextAccessor.HttpContext.GetUser();
        if (user == null)
        {
          context.Fail(new AuthorizationFailureReason(this, "The User is required."));
        }
        else if (!_administrators.Contains(user.Username.ToUpper()))
        {
          context.Fail(new AuthorizationFailureReason(this, "The user is not an administrator."));
        }
        else
        {
          context.Succeed(requirement);
        }
      }

      return Task.CompletedTask;
    }
  }
}
