using MediatR;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Seeding.Worker.Backend.Tasks;

internal class SeedAbilitiesTask : SeedingTask
{
  public override string? Description => "Seeds the Pokémon abilities.";
}

internal class SeedAbilitiesTaskHandler : INotificationHandler<SeedAbilitiesTask>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedAbilitiesTaskHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private const string UserIdKey = "UserId";

  private readonly UserId _userId;
  private readonly IAbilityRepository _abilityRepository;
  private readonly ILogger<SeedAbilitiesTaskHandler> _logger;

  public SeedAbilitiesTaskHandler(IAbilityRepository abilityRepository, IConfiguration configuration, ILogger<SeedAbilitiesTaskHandler> logger)
  {
    string userId = configuration.GetValue<string>(UserIdKey) ?? throw new InvalidOperationException($"The configuration '{UserIdKey}' is required.");
    _userId = new(userId);

    _abilityRepository = abilityRepository;
    _logger = logger;
  }

  public async Task Handle(SeedAbilitiesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/abilities.json", Encoding.UTF8, cancellationToken);
    IEnumerable<AbilitySummary>? summaries = JsonSerializer.Deserialize<IEnumerable<AbilitySummary>>(json, _serializerOptions);
    if (summaries != null)
    {
      IEnumerable<AbilityId> ids = summaries.Select(x => new AbilityId(x.Id)).Distinct();
      Dictionary<AbilityId, Ability> abilities = (await _abilityRepository.LoadAsync(ids, cancellationToken))
        .ToDictionary(x => x.Id, x => x);

      foreach (AbilitySummary summary in summaries)
      {
        string status = "created";
        AbilityId id = new(summary.Id);
        if (abilities.TryGetValue(id, out Ability? ability))
        {
          status = "updated";
        }
        else
        {
          ability = new(new Name(summary.Name), _userId, id);
          abilities[id] = ability;
        }

        ability.Kind = summary.Kind;
        ability.Name = new Name(summary.Name);
        ability.Description = Description.TryCreate(summary.Description);
        ability.Link = Url.TryCreate(summary.Link);
        ability.Notes = Notes.TryCreate(summary.Notes);

        ability.Update(_userId);
        _logger.LogInformation("The ability '{Name}' has been {Status} (Id={Id}).", ability.Name, status, ability.Id.ToGuid());
      }

      await _abilityRepository.SaveAsync(abilities.Values, cancellationToken);
    }
  }
}
