using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PokeGame.Infrastructure.Commands;

namespace PokeGame.EntityFrameworkCore.Handlers;

internal class InitializeDatabaseCommandHandler : INotificationHandler<InitializeDatabaseCommand>
{
  private readonly bool _enableMigrations;
  private readonly EventContext _eventContext;
  private readonly PokemonContext _pokemonContext;

  public InitializeDatabaseCommandHandler(IConfiguration configuration, EventContext eventContext, PokemonContext pokemonContext)
  {
    _enableMigrations = configuration.GetValue<bool>("EnableMigrations");
    _eventContext = eventContext;
    _pokemonContext = pokemonContext;
  }

  public async Task Handle(InitializeDatabaseCommand _, CancellationToken cancellationToken)
  {
    if (_enableMigrations)
    {
      await _eventContext.Database.MigrateAsync(cancellationToken);
      await _pokemonContext.Database.MigrateAsync(cancellationToken);
    }
  }
}
