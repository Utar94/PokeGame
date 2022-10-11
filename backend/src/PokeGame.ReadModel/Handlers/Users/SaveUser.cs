using Logitar.Portal.Core.Users.Models;
using MediatR;

namespace PokeGame.ReadModel.Handlers.Users
{
  public class SaveUser : INotification
  {
    public SaveUser(UserModel user)
    {
      User = user;
    }

    public UserModel User { get; }
  }
}
