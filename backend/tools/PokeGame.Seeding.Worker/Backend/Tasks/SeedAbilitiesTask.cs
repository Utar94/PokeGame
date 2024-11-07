using MediatR;
using PokeGame.Application;
using PokeGame.Application.Abilities.Commands;
using PokeGame.Contracts.Abilities;

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

  private readonly ILogger<SeedAbilitiesTaskHandler> _logger;
  private readonly IRequestPipeline _pipeline;

  public SeedAbilitiesTaskHandler(ILogger<SeedAbilitiesTaskHandler> logger, IRequestPipeline pipeline)
  {
    _logger = logger;
    _pipeline = pipeline;
  }

  public async Task Handle(SeedAbilitiesTask _, CancellationToken cancellationToken)
  {
    string json = await File.ReadAllTextAsync("Backend/abilities.json", Encoding.UTF8, cancellationToken);
    IEnumerable<AbilityPayload>? payloads = JsonSerializer.Deserialize<IEnumerable<AbilityPayload>>(json, _serializerOptions);
    if (payloads != null)
    {
      foreach (AbilityPayload payload in payloads)
      {
        CreateOrReplaceAbilityCommand command = new(payload.Id, payload, Version: null);
        CreateOrReplaceAbilityResult result = await _pipeline.ExecuteAsync(command, cancellationToken);
        AbilityModel ability = result.Ability ?? throw new InvalidOperationException("The ability model should not be null.");
        string status = result.Created ? "created" : "updated";
        _logger.LogInformation("The ability '{UniqueName}' has been {Status} (Id={Id}).", ability.UniqueName, status, ability.Id);
      }
    }
  }
}
