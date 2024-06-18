using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Seeding.Worker.Pokemon.Commands;

internal class SeedRegionsCommandHandler : INotificationHandler<SeedRegionsCommand>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedRegionsCommandHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private readonly ILogger<SeedRegionsCommandHandler> _logger;
  private readonly IPokemonClient _pokemon;

  public SeedRegionsCommandHandler(ILogger<SeedRegionsCommandHandler> logger, IPokemonClient pokemon)
  {
    _logger = logger;
    _pokemon = pokemon;
  }

  public async Task Handle(SeedRegionsCommand _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Pokemon/Data/regions.json", Encoding.UTF8, cancellationToken);
    IEnumerable<RegionSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<RegionSummary>>(json, _serializerOptions);
    if (summaries != null)
    {
      SearchResults<Region> results = await _pokemon.SearchRegionsAsync(new SearchRegionsPayload(), cancellationToken);
      Dictionary<string, Region> regions = new(capacity: results.Items.Count);
      foreach (Region item in results.Items)
      {
        regions[StringHelper.Normalize(item.UniqueName)] = item;
      }

      foreach (RegionSummary summary in summaries)
      {
        string status = "created";
        string key = StringHelper.Normalize(summary.UniqueName);
        if (regions.TryGetValue(key, out Region? region))
        {
          status = "updated";
        }
        else
        {
          CreateRegionPayload createPayload = new(summary.UniqueName);
          region = await _pokemon.CreateRegionAsync(createPayload, cancellationToken);
          regions[key] = region;
        }

        ReplaceRegionPayload replacePayload = new(summary.UniqueName)
        {
          DisplayName = summary.DisplayName,
          Description = summary.Description,
          Reference = summary.Reference,
          Notes = summary.Notes
        };
        region = await _pokemon.ReplaceRegionAsync(region.Id, replacePayload, region.Version, cancellationToken)
          ?? throw new InvalidOperationException($"The region 'Id={region.Id}' update result should not be null.");

        _logger.LogInformation("The region '{Name}' has been {Status} (Id={Id}).", region.DisplayName ?? region.UniqueName, status, region.Id);
      }
    }
  }
}
