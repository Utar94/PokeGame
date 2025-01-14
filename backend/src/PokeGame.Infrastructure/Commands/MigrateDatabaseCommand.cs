using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PokeGame.Infrastructure.Commands;

public record MigrateDatabaseCommand : INotification;

internal class MigrateDatabaseCommandHandler : INotificationHandler<MigrateDatabaseCommand>
{
  private readonly EventContext _eventContext;
  private readonly PokeGameContext _pokeGameContext;

  public MigrateDatabaseCommandHandler(EventContext eventContext, PokeGameContext pokeGameContext)
  {
    _eventContext = eventContext;
    _pokeGameContext = pokeGameContext;
  }

  public async Task Handle(MigrateDatabaseCommand _, CancellationToken cancellationToken)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _pokeGameContext.Database.MigrateAsync(cancellationToken);
  }
}
