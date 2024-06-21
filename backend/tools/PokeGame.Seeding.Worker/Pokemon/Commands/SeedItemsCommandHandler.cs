using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Seeding.Worker.Pokemon.Commands;

internal class SeedItemsCommandHandler : INotificationHandler<SeedItemsCommand>
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static SeedItemsCommandHandler()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
  }

  private readonly ILogger<SeedItemsCommandHandler> _logger;
  private readonly IPokemonClient _pokemon;

  public SeedItemsCommandHandler(ILogger<SeedItemsCommandHandler> logger, IPokemonClient pokemon)
  {
    _logger = logger;
    _pokemon = pokemon;
  }

  public async Task Handle(SeedItemsCommand _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Pokemon/Data/items.json", Encoding.UTF8, cancellationToken);
    IEnumerable<ItemSummary>? summaries = JsonSerializer.Deserialize<IEnumerable<ItemSummary>>(json, _serializerOptions);
    if (summaries != null)
    {
      SearchResults<Item> results = await _pokemon.SearchItemsAsync(new SearchItemsPayload(), cancellationToken);
      Dictionary<string, Item> items = new(capacity: results.Items.Count);
      foreach (Item item in results.Items)
      {
        items[StringHelper.Normalize(item.UniqueName)] = item;
      }

      foreach (ItemSummary summary in summaries)
      {
        string status = "created";
        string key = StringHelper.Normalize(summary.UniqueName);
        if (items.TryGetValue(key, out Item? item))
        {
          status = "updated";
        }
        else
        {
          CreateItemPayload createPayload = new(summary.UniqueName)
          {
            Category = summary.Category,
            Medicine = summary.Medicine,
            PokeBall = summary.PokeBall
          };
          item = await _pokemon.CreateItemAsync(createPayload, cancellationToken);
          items[key] = item;
        }

        ReplaceItemPayload replacePayload = new(summary.UniqueName)
        {
          Price = summary.Price,
          DisplayName = summary.DisplayName,
          Description = summary.Description,
          Picture = summary.Picture,
          Medicine = summary.Medicine,
          PokeBall = summary.PokeBall,
          Reference = summary.Reference,
          Notes = summary.Notes
        };
        item = await _pokemon.ReplaceItemAsync(item.Id, replacePayload, item.Version, cancellationToken)
          ?? throw new InvalidOperationException($"The item 'Id={item.Id}' update result should not be null.");

        _logger.LogInformation("The item '{Name}' has been {Status} (Id={Id}).", item.DisplayName ?? item.UniqueName, status, item.Id);
      }
    }
  }
}
