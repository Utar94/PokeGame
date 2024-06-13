using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Seeding.Worker.Pokemon.Commands;

internal class SeedAbilitiesCommandHandler : INotificationHandler<SeedAbilitiesCommand>
{
  private readonly ILogger<SeedAbilitiesCommandHandler> _logger;
  private readonly IPokemonClient _pokemon;

  public SeedAbilitiesCommandHandler(ILogger<SeedAbilitiesCommandHandler> logger, IPokemonClient pokemon)
  {
    _logger = logger;
    _pokemon = pokemon;
  }

  public async Task Handle(SeedAbilitiesCommand _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Pokemon/Data/abilities.json", Encoding.UTF8, cancellationToken);
    IEnumerable<AbilitySummary>? summaries = JsonSerializer.Deserialize<IEnumerable<AbilitySummary>>(json);
    if (summaries != null)
    {
      SearchResults<Ability> results = await _pokemon.SearchAbilitiesAsync(new SearchAbilitiesPayload(), cancellationToken);
      Dictionary<string, Ability> abilities = new(capacity: results.Items.Count);
      foreach (Ability item in results.Items)
      {
        abilities[StringHelper.Normalize(item.UniqueName)] = item;
      }

      foreach (AbilitySummary summary in summaries)
      {
        string status = "created";
        string key = StringHelper.Normalize(summary.UniqueName);
        if (abilities.TryGetValue(key, out Ability? ability))
        {
          status = "updated";
        }
        else
        {
          CreateAbilityPayload createPayload = new(summary.UniqueName);
          ability = await _pokemon.CreateAbilityAsync(createPayload, cancellationToken);
          abilities[key] = ability;
        }

        ReplaceAbilityPayload replacePayload = new(summary.UniqueName)
        {
          DisplayName = summary.DisplayName,
          Description = summary.Description,
          Reference = summary.Reference,
          Notes = summary.Notes
        };
        ability = await _pokemon.ReplaceAbilityAsync(ability.Id, replacePayload, ability.Version, cancellationToken)
          ?? throw new InvalidOperationException($"The ability 'Id={ability.Id}' update result should not be null.");

        _logger.LogInformation("The ability '{Name}' has been {Status} (Id={Id}).", ability.DisplayName ?? ability.UniqueName, status, ability.Id);
      }
    }
  }
}
