using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Seeding.Worker.Pokemon.Commands;

internal class SeedMovesCommandHandler : INotificationHandler<SeedMovesCommand>
{
  private static readonly JsonSerializerOptions _serializerOptions;
  static SeedMovesCommandHandler()
  {
    _serializerOptions = new();
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private readonly ILogger<SeedMovesCommandHandler> _logger;
  private readonly IPokemonClient _pokemon;

  public SeedMovesCommandHandler(ILogger<SeedMovesCommandHandler> logger, IPokemonClient pokemon)
  {
    _logger = logger;
    _pokemon = pokemon;
  }

  public async Task Handle(SeedMovesCommand _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Pokemon/Data/moves.json", Encoding.UTF8, cancellationToken);
    IEnumerable<MoveSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<MoveSummary>>(json, _serializerOptions);
    if (summaries != null)
    {
      SearchResults<Move> results = await _pokemon.SearchMovesAsync(new SearchMovesPayload(), cancellationToken);
      Dictionary<string, Move> moves = new(capacity: results.Items.Count);
      foreach (Move item in results.Items)
      {
        moves[StringHelper.Normalize(item.UniqueName)] = item;
      }

      foreach (MoveSummary summary in summaries)
      {
        string status = "created";
        string key = StringHelper.Normalize(summary.UniqueName);
        if (moves.TryGetValue(key, out Move? move))
        {
          status = "updated";
        }
        else
        {
          CreateMovePayload createPayload = new(summary.UniqueName)
          {
            Type = summary.Type,
            Category = summary.Category,
            PowerPoints = 1
          };
          move = await _pokemon.CreateMoveAsync(createPayload, cancellationToken);
          moves[key] = move;
        }

        ReplaceMovePayload replacePayload = new(summary.UniqueName)
        {
          DisplayName = summary.DisplayName,
          Description = summary.Description,
          Accuracy = summary.Accuracy,
          Power = summary.Power,
          PowerPoints = summary.PowerPoints,
          Reference = summary.Reference,
          Notes = summary.Notes
        };
        replacePayload.StatisticChanges.AddRange(summary.StatisticChanges);
        replacePayload.StatusConditions.AddRange(summary.StatusConditions);
        move = await _pokemon.ReplaceMoveAsync(move.Id, replacePayload, move.Version, cancellationToken)
          ?? throw new InvalidOperationException($"The move 'Id={move.Id}' update result should not be null.");

        _logger.LogInformation("The move '{Name}' has been {Status} (Id={Id}).", move.DisplayName ?? move.UniqueName, status, move.Id);
      }
    }
  }
}
