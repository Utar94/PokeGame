using Bogus;
using Logitar.Data;
using Logitar.Data.PostgreSQL;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Sessions;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PokeGame.Application;
using PokeGame.Application.Accounts.Events;
using PokeGame.Application.Logging;
using PokeGame.Domain;
using PokeGame.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Infrastructure;
using System.Text;
using Locale = Logitar.Portal.Contracts.Locale;
using PokeGameDb = PokeGame.EntityFrameworkCore.PokeGameDb;

namespace PokeGame;

public abstract class IntegrationTests : IAsyncLifetime
{
  private readonly DatabaseProvider _databaseProvider;
  private readonly User _user;

  protected Actor Actor { get; }
  protected Faker Faker { get; } = new();
  protected UserId UserId { get; }

  protected IConfiguration Configuration { get; }
  protected IServiceProvider ServiceProvider { get; }

  protected IRequestPipeline Pipeline { get; }
  protected EventContext EventContext { get; }
  protected PokeGameContext PokeGameContext { get; }

  protected IntegrationTests()
  {
    Configuration = new ConfigurationBuilder()
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
      .Build();

    ServiceCollection services = new();
    services.AddSingleton(Configuration);

    string connectionString;
    string database = GetType().Name;
    _databaseProvider = Configuration.GetValue<DatabaseProvider?>("DatabaseProvider") ?? DatabaseProvider.EntityFrameworkCoreSqlServer;
    switch (_databaseProvider)
    {
      case DatabaseProvider.EntityFrameworkCorePostgreSQL:
        connectionString = Configuration.GetValue<string>("POSTGRESQLCONNSTR_PokeGame")?.Replace("{Database}", database) ?? string.Empty;
        services.AddPokeGameWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      case DatabaseProvider.EntityFrameworkCoreSqlServer:
        connectionString = Configuration.GetValue<string>("SQLCONNSTR_PokeGame")?.Replace("{Database}", database) ?? string.Empty;
        services.AddPokeGameWithEntityFrameworkCoreSqlServer(connectionString);
        break;
      default:
        throw new DatabaseProviderNotSupportedException(_databaseProvider);
    }

    DateTime now = DateTime.UtcNow;
    _user = new(Faker.Person.Email)
    {
      Id = Guid.NewGuid(),
      CreatedOn = now,
      UpdatedOn = now,
      Email = new Email(Faker.Person.Email)
      {
        IsVerified = true,
        VerifiedOn = now
      },
      IsConfirmed = true,
      FirstName = Faker.Person.FirstName,
      LastName = Faker.Person.LastName,
      FullName = Faker.Person.FullName,
      Locale = new Locale(Faker.Locale),
      TimeZone = "America/Montreal",
      Picture = Faker.Person.Avatar,
      AuthenticatedOn = now
    };
    Actor = new(_user);
    _user.CreatedBy = Actor;
    _user.UpdatedBy = Actor;
    _user.Email.VerifiedBy = Actor;
    UserId = new(_user.Id);

    ActivityContext activityContext = new(Session: null, _user);
    services.AddSingleton<IActivityContextResolver>(new TestActivityContextResolver(activityContext));
    services.AddSingleton<ILogRepository, FakeLogRepository>();

    ServiceProvider = services.BuildServiceProvider();

    Pipeline = ServiceProvider.GetRequiredService<IRequestPipeline>();
    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    PokeGameContext = ServiceProvider.GetRequiredService<PokeGameContext>();
  }

  public virtual async Task InitializeAsync()
  {
    await MigrateAsync();
    await EmptyDatabaseAsync();
    await SeedDatabaseAsync();
  }

  private async Task MigrateAsync()
  {
    await EventContext.Database.MigrateAsync();
    await PokeGameContext.Database.MigrateAsync();
  }

  private async Task EmptyDatabaseAsync()
  {
    StringBuilder statement = new();
    TableId[] tables =
    [
      PokeGameDb.Regions.Table,
      //PokeGameDb.Moves.Table, // TODO(fpion): complete
      PokeGameDb.Abilities.Table,
      PokeGameDb.Users.Table,
      EventDb.Events.Table
    ];
    foreach (TableId table in tables)
    {
      statement.Append(CreateDeleteBuilder(table).Build().Text);
    }
    await PokeGameContext.Database.ExecuteSqlRawAsync(statement.ToString());
  }
  private IDeleteBuilder CreateDeleteBuilder(TableId table)
  {
    return _databaseProvider switch
    {
      DatabaseProvider.EntityFrameworkCorePostgreSQL => new PostgresDeleteBuilder(table),
      DatabaseProvider.EntityFrameworkCoreSqlServer => new SqlServerDeleteBuilder(table),
      _ => throw new DatabaseProviderNotSupportedException(_databaseProvider),
    };
  }

  private async Task SeedDatabaseAsync()
  {
    Session session = new(_user);
    UserSignedInEvent @event = new(session);

    IPublisher publisher = ServiceProvider.GetRequiredService<IPublisher>();
    await publisher.Publish(@event);
  }

  public virtual Task DisposeAsync() => Task.CompletedTask;
}
