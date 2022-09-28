using Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Models.Users
{
  public class CurrentUser
  {
    public CurrentUser(UserModel? user = null, bool isAdministrator = false)
    {
      IsAdministrator = isAdministrator;
      IsAuthenticated = user != null;

      Email = user?.Email;
      FullName = user?.FullName;
      Picture = user?.Picture;
      Username = user?.Username;
    }

    public bool IsAdministrator { get; }
    public bool IsAuthenticated { get; }

    public string? Email { get; }
    public string? FullName { get; }
    public string? Picture { get; }
    public string? Username { get; }
  }
}
