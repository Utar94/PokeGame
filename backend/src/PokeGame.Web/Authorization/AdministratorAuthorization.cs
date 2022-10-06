using Logitar.Portal.Core.Users.Models;
using Microsoft.AspNetCore.Authorization;

namespace PokeGame.Web.Authorization
{
  internal class AdministratorAuthorizationRequirement : IAuthorizationRequirement
  {
  }

  internal class AdministratorAuthorizationHandler : AuthorizationHandler<AdministratorAuthorizationRequirement>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserService _userService;

    public AdministratorAuthorizationHandler(IHttpContextAccessor httpContextAccessor, UserService userService)
    {
      _httpContextAccessor = httpContextAccessor;
      _userService = userService;
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
        else if (!_userService.IsAdministrator(user))
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
