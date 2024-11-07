using MediatR;
using PokeGame.Application;
using PokeGame.Application.Regions.Commands;
using PokeGame.Contracts.Regions;

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

  private readonly ILogger<SeedRegionsTaskHandler> _logger;
  private readonly IRequestPipeline _pipeline;

  public SeedRegionsTaskHandler(ILogger<SeedRegionsTaskHandler> logger, IRequestPipeline pipeline)
  {
    _logger = logger;
    _pipeline = pipeline;
  }

  public async Task Handle(SeedRegionsTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/regions.json", Encoding.UTF8, cancellationToken);
    IEnumerable<RegionPayload>? payloads = JsonSerializer.Deserialize<IEnumerable<RegionPayload>>(json, _serializerOptions);
    if (payloads != null)
    {
      foreach (RegionPayload payload in payloads)
      {
        CreateOrReplaceRegionCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceRegionResult result = await _pipeline.ExecuteAsync(command, cancellationToken);
        RegionModel region = result.Region ?? throw new InvalidOperationException("The region model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The region '{Name}' has been {Status} (Id={Id}).", region.UniqueName, status, region.Id);
      }
    }
  }
}
