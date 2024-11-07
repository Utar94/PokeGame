using MediatR;
using PokeGame.Application;
using PokeGame.Application.Moves.Commands;
using PokeGame.Contracts.Moves;

namespace PokeGame.Seeding.Worker.Backend.Tasks;

internal class SeedMovesTask : SeedingTask
{
  public override string? Description => "Seeds the Pokémon moves.";
}

internal class SeedMovesTaskHandler : INotificationHandler<SeedMovesTask>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedMovesTaskHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private readonly ILogger<SeedMovesTaskHandler> _logger;
  private readonly IRequestPipeline _pipeline;

  public SeedMovesTaskHandler(ILogger<SeedMovesTaskHandler> logger, IRequestPipeline pipeline)
  {
    _logger = logger;
    _pipeline = pipeline;
  }

  public async Task Handle(SeedMovesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/moves.json", Encoding.UTF8, cancellationToken);
    IEnumerable<MovePayload>? payloads = JsonSerializer.Deserialize<IEnumerable<MovePayload>>(json, _serializerOptions);
    if (payloads != null)
    {
      foreach (MovePayload payload in payloads)
      {
        CreateOrReplaceMoveCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceMoveResult result = await _pipeline.ExecuteAsync(command, cancellationToken);
        MoveModel move = result.Move ?? throw new InvalidOperationException("The move model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The move '{UniqueName}' has been {Status} (Id={Id}).", move.UniqueName, status, move.Id);
      }
    }
  }
}
