using Logitar.Portal.Core.Users.Models;
using PokeGame.Web.Models.Users;

namespace PokeGame.Web
{
  public class UserService
  {
    private readonly HashSet<string> _administrators;

    public UserService(IConfiguration configuration)
    {
      _administrators = configuration.GetSection("Administrators").Get<IEnumerable<string>>()
        .Select(x => x.ToUpper())
        .ToHashSet();
    }

    public CurrentUser GetCurrentUser(HttpContext httpContext)
    {
      UserModel? user = httpContext.GetUser();

      return new(user, user != null && IsAdministrator(user));
    }

    public bool IsAdministrator(UserModel user) => _administrators.Contains(user.Username.ToUpper());
  }
}
