using Bogus;
using Logitar.Data.SqlServer;
using Logitar.EventSourcing;
using Logitar.EventSourcing.EntityFrameworkCore.Relational;
using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Actors;
using Logitar.Portal.Contracts.Realms;
using Logitar.Portal.Contracts.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using PokeGame.Application;
using PokeGame.Application.Accounts;
using PokeGame.Contracts.Accounts;
using PokeGame.EntityFrameworkCore;
using PokeGame.EntityFrameworkCore.Entities;
using PokeGame.EntityFrameworkCore.SqlServer;
using PokeGame.Infrastructure;
using PokeGame.Infrastructure.Commands;
using System.Globalization;
using System.Text;

namespace PokeGame;

public abstract class IntegrationTests : IAsyncLifetime
{
  private readonly TestContext _context = new();

  protected CancellationToken CancellationToken { get; }
  protected Faker Faker { get; } = new();

  protected IConfiguration Configuration { get; }
  protected IServiceProvider ServiceProvider { get; }

  protected IRequestPipeline Pipeline { get; }
  protected EventContext EventContext { get; }
  protected PokemonContext PokemonContext { get; }

  protected Mock<IApiKeyService> ApiKeyService { get; } = new();
  protected Mock<IMessageService> MessageService { get; } = new();
  protected Mock<IOneTimePasswordService> OneTimePasswordService { get; } = new();
  protected Mock<IRealmService> RealmService { get; } = new();
  protected Mock<ISessionService> SessionService { get; } = new();
  protected Mock<ITokenService> TokenService { get; } = new();
  protected Mock<IUserService> UserService { get; } = new();

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

    services.AddSingleton(ApiKeyService.Object);
    services.AddSingleton(MessageService.Object);
    services.AddSingleton(OneTimePasswordService.Object);
    services.AddSingleton(RealmService.Object);
    services.AddSingleton(SessionService.Object);
    services.AddSingleton(TokenService.Object);
    services.AddSingleton(UserService.Object);

    ServiceProvider = services.BuildServiceProvider();

    Pipeline = ServiceProvider.GetRequiredService<IRequestPipeline>();
    EventContext = ServiceProvider.GetRequiredService<EventContext>();
    PokemonContext = ServiceProvider.GetRequiredService<PokemonContext>();

    Realm realm = new("pokegame", "E(Y,`PM;-.25#49Bphzqa%W}rumQ+FHf")
    {
      DisplayName = "PokéGame",
      DefaultLocale = new Locale("fr"),
      Url = "https://app.pokegame.ca/"
    };
    RealmService.Setup(x => x.FindAsync(CancellationToken)).ReturnsAsync(realm);

    DateTime now = DateTime.Now;
    User user = new(Faker.Person.Email)
    {
      Id = Guid.NewGuid(),
      Version = 1,
      CreatedOn = now,
      UpdatedOn = now,
      HasPassword = true,
      PasswordChangedOn = now,
      Email = new Email(Faker.Person.Email)
      {
        IsVerified = true,
        VerifiedOn = now
      },
      IsConfirmed = true,
      FirstName = Faker.Person.FirstName,
      LastName = Faker.Person.LastName,
      FullName = Faker.Person.FullName,
      Birthdate = Faker.Person.DateOfBirth,
      Gender = Faker.Person.Gender.ToString().ToLowerInvariant(),
      Locale = new Locale("fr"),
      TimeZone = "America/Toronto",
      Picture = Faker.Person.Avatar,
      Website = $"https://www.{Faker.Person.Website}",
      AuthenticatedOn = now,
      Realm = realm
    };
    user.CustomAttributes.Add(new CustomAttribute("MultiFactorAuthenticationMode", MultiFactorAuthenticationMode.Email.ToString()));
    user.CustomAttributes.Add(new CustomAttribute("ProfileCompletedOn", now.ToString("O", CultureInfo.InvariantCulture)));
    _context.User = user;
    user.CreatedBy = Actor;
    user.UpdatedBy = Actor;
    user.PasswordChangedBy = Actor;
    user.Email.VerifiedBy = Actor;
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

    if (_context.User != null)
    {
      ActorEntity actor = new(_context.User);
      PokemonContext.Actors.Add(actor);
      await PokemonContext.SaveChangesAsync();
    }
  }

  public virtual Task DisposeAsync() => Task.CompletedTask;
}
