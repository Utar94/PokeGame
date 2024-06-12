using Logitar.Portal.Contracts;
using Logitar.Portal.Contracts.Realms;
using MediatR;

namespace PokeGame.Seeding.Worker.Portal.Commands;

internal class SeedRealmCommandHandler : INotificationHandler<SeedRealmCommand>
{
  private readonly ILogger<SeedRealmCommandHandler> _logger;
  private readonly IRealmClient _realms;
  private readonly string _uniqueSlug;

  public SeedRealmCommandHandler(IConfiguration configuration, ILogger<SeedRealmCommandHandler> logger, IRealmClient realms)
  {
    _logger = logger;
    _realms = realms;
    _uniqueSlug = configuration.GetValue<string>("Portal:Realm") ?? throw new ArgumentException("The configuration 'Portal:Realm' is required.", nameof(configuration));
  }

  public async Task Handle(SeedRealmCommand _, CancellationToken cancellationToken)
  {
    RequestContext context = new(cancellationToken);

    string status = "updated";
    Realm? realm = await _realms.ReadAsync(id: null, _uniqueSlug, context);
    if (realm == null)
    {
      CreateRealmPayload createPayload = new(_uniqueSlug, secret: string.Empty);
      realm = await _realms.CreateAsync(createPayload, context);
      status = "created";
    }

    ReplaceRealmPayload replacePayload = new(realm.UniqueSlug, realm.Secret)
    {
      DisplayName = "PokéGame",
      Description = "This is the realm of the Pokémon game management system.",
      DefaultLocale = "en",
      Url = "http://localhost:7792"
    };
    realm = await _realms.ReplaceAsync(realm.Id, replacePayload, realm.Version, context)
      ?? throw new InvalidOperationException($"The realm 'Id={realm.Id}' replace result should not be null.");

    WorkerPortalSettings.Instance.SetRealm(realm);

    _logger.LogInformation("The realm '{UniqueSlug}' has been {Status} (Id={Id}).", realm.UniqueSlug, status, realm.Id);
  }
}
