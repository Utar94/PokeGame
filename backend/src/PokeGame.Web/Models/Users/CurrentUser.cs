using Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Models.Users
{
  internal class CurrentUser
  {
    public CurrentUser(UserModel? user)
    {
      IsAuthenticated = user != null;

      Email = user?.Email;
      FullName = user?.FullName;
      Picture = user?.Picture;
      Username = user?.Username;
    }

    public bool IsAuthenticated { get; }

    public string? Email { get; }
    public string? FullName { get; }
    public string? Picture { get; }
    public string? Username { get; }
  }
}
