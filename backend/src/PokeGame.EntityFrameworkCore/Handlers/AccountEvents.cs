using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Accounts.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal static class AccountEvents
{
  public class UserSignedInEventHandler : INotificationHandler<UserSignedInEvent>
  {
    private readonly PokeGameContext _context;

    public UserSignedInEventHandler(PokeGameContext context)
    {
      _context = context;
    }

    public async Task Handle(UserSignedInEvent @event, CancellationToken cancellationToken)
    {
      User user = @event.Session.User;
      UserEntity? entity = await _context.Users.SingleOrDefaultAsync(x => x.Id == user.Id, cancellationToken);
      if (entity == null)
      {
        entity = new(user);
        _context.Users.Add(entity);
      }
      else
      {
        entity.Update(user);
      }

      await _context.SaveChangesAsync(cancellationToken);
    }
  }
}
