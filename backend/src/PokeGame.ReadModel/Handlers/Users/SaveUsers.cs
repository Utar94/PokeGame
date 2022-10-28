using Logitar.Portal.Core;
using Logitar.Portal.Core.Users.Models;
using MediatR;

namespace PokeGame.ReadModel.Handlers.Users
{
  public class SaveUsers : INotification
  {
    public SaveUsers(ListModel<UserSummary> users)
    {
      Users = users;
    }

    public ListModel<UserSummary> Users { get; }
  }
}
