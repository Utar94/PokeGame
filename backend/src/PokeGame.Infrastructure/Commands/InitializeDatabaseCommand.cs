﻿using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace PokeGame.Infrastructure.Commands;

public record InitializeDatabaseCommand : INotification;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly EventContext _eventContext;
  private readonly PokeGameContext _pokeGameContext;

  public InitializeDatabaseCommandHandler(EventContext eventContext, PokeGameContext pokeGameContext)
  {
    _eventContext = eventContext;
    _pokeGameContext = pokeGameContext;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    await _eventContext.Database.MigrateAsync(cancellationToken);
    await _pokeGameContext.Database.MigrateAsync(cancellationToken);
  }
}
