using Logitar.EventSourcing;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Accounts.Events;
using PokeGame.EntityFrameworkCore.Entities;

namespace PokeGame.EntityFrameworkCore.Handlers.Accounts;

internal class AccountSignedInEventHandler : INotificationHandler<AccountSignedInEvent>
{
  private readonly PokemonContext _context;

  public AccountSignedInEventHandler(PokemonContext context)
  {
    _context = context;
  }

  public async Task Handle(AccountSignedInEvent @event, CancellationToken cancellationToken)
  {
    User user = @event.Session.User;

    string id = new ActorId(user.Id).Value;
    ActorEntity? actor = await _context.Actors.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
    if (actor == null)
    {
      actor = new(user);

      _context.Actors.Add(actor);
    }
    else
    {
      actor.Update(user);
    }

    await _context.SaveChangesAsync(cancellationToken);
  }
}
