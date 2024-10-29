using MediatR;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Seeding.Worker.Backend.Tasks;

internal class SeedRegionsTask : SeedingTask
{
  public override string? Description => "Seeds the Pokémon regions.";
}

internal class SeedRegionsTaskHandler : INotificationHandler<SeedRegionsTask>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedRegionsTaskHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private const string UserIdKey = "UserId";

  private readonly UserId _userId;
  private readonly ILogger<SeedRegionsTaskHandler> _logger;
  private readonly IRegionRepository _regionRepository;

  public SeedRegionsTaskHandler(IConfiguration configuration, ILogger<SeedRegionsTaskHandler> logger, IRegionRepository regionRepository)
  {
    string userId = configuration.GetValue<string>(UserIdKey) ?? throw new InvalidOperationException($"The configuration '{UserIdKey}' is required.");
    _userId = new(userId);

    _logger = logger;
    _regionRepository = regionRepository;
  }

  public async Task Handle(SeedRegionsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/regions.json", Encoding.UTF8, cancellationToken);
    IEnumerable<RegionSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<RegionSummary>>(json, _serializerOptions);
    if (summaries != null)
    {
      IEnumerable<RegionId> ids = summaries.Select(x => new RegionId(x.Id)).Distinct();
      Dictionary<RegionId, Region> regions = (await _regionRepository.LoadAsync(ids, cancellationToken))
        .ToDictionary(x => x.Id, x => x);

      foreach (RegionSummary summary in summaries)
      {
        string status = "created";
        RegionId id = new(summary.Id);
        if (regions.TryGetValue(id, out Region? region))
        {
          status = "updated";
        }
        else
        {
          region = new(new Name(summary.Name), _userId, id);
          regions[id] = region;
        }

        region.Name = new Name(summary.Name);
        region.Description = Description.TryCreate(summary.Description);
        region.Link = Url.TryCreate(summary.Link);
        region.Notes = Notes.TryCreate(summary.Notes);

        region.Update(_userId);
        _logger.LogInformation("The region '{Name}' has been {Status} (Id={Id}).", region.Name, status, region.Id.ToGuid());
      }

      await _regionRepository.SaveAsync(regions.Values, cancellationToken);
    }
  }
}
