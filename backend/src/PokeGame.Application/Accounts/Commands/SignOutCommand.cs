using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using MediatR;

namespace PokeGame.Application.Accounts.Commands;

public record SignOutCommand : Activity, IRequest<Unit>
{
  public Guid? SessionId { get; }
  public Guid? UserId { get; }

  public SignOutCommand(Session session)
  {
    SessionId = session.Id;
  }
  public SignOutCommand(User user)
  {
    UserId = user.Id;
  }
}
