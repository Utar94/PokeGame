using Bogus;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Contracts.Actors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Infrastructure;
using PokeGame.Infrastructure.Commands;
using System.Text;

namespace PokeGame;

public abstract class IntegrationTests : IAsyncLifetime
{
  private readonly TestContext _context = new();

  protected Faker Faker { get; } = new();

  protected IConfiguration Configuration { get; }
  protected IServiceProvider ServiceProvider { get; }

  protected IRequestPipeline Pipeline { get; }
  protected EventContext EventContext { get; }
  protected PokemonContext PokemonContext { get; }

  public virtual Actor Actor
  {
    get
    {
      if (_context.User != null)
      {
        return new Actor(_context.User);
      }

      return Actor.System;
    }
  }
  public ActorId ActorId => new(Actor.Id);

  protected IntegrationTests()
  {
    Configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    ServiceCollection services = new();
    services.AddSingleton(Configuration);
    services.AddSingleton(_context);
    services.AddSingleton<IActivityContextResolver, TestActivityContextResolver>();

    string connectionString;
    DatabaseProvider databaseProvider = Configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = Configuration.GetValue<string>("SQLCONNSTR_Pokemon")?.Replace("{Database}", GetType().Name) ?? string.Empty;
        services.AddPokeGameWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(databaseProvider);
    }

    ServiceProvider = services.BuildServiceProvider();

    Pipeline = ServiceProvider.GetRequiredService<IRequestPipeline>();
    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    PokemonContext = ServiceProvider.GetRequiredService<PokemonContext>();
  }

  public virtual async Task InitializeAsync()
  {
    IPublisher publisher = ServiceProvider.GetRequiredService<IPublisher>();
    await publisher.Publish(new InitializeDatabaseCommand());

    StringBuilder statement = new();
    statement.AppendLine(SqlServerDeleteBuilder.From(PokemonDb.Abilities.Table).Build().Text);
    statement.AppendLine(SqlServerDeleteBuilder.From(PokemonDb.Actors.Table).Build().Text);
    statement.AppendLine(SqlServerDeleteBuilder.From(EventDb.Events.Table).Build().Text);
    await PokemonContext.Database.ExecuteSqlRawAsync(statement.ToString());
  }

  public virtual Task DisposeAsync() => Task.CompletedTask;
}
