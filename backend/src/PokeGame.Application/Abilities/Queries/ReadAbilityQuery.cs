using MediatR;
using PokeGame.Application.Abilities.Models;

namespace PokeGame.Application.Abilities.Queries;

public record ReadAbilityQuery(Guid? Id, string? UniqueName) : IRequest<AbilityModel?>;

internal class ReadAbilityQueryHandler : IRequestHandler<ReadAbilityQuery, AbilityModel?>
{
  private readonly IAbilityQuerier _abilityQuerier;

  public ReadAbilityQueryHandler(IAbilityQuerier abilityQuerier)
  {
    _abilityQuerier = abilityQuerier;
  }

  public async Task<AbilityModel?> Handle(ReadAbilityQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, AbilityModel> abilities = new(capacity: 2);

    if (query.Id.HasValue)
    {
      AbilityModel? ability = await _abilityQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (ability != null)
      {
        abilities[ability.Id] = ability;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      AbilityModel? ability = await _abilityQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (ability != null)
      {
        abilities[ability.Id] = ability;
      }
    }

    if (abilities.Count > 1)
    {
      throw TooManyResultsException<AbilityModel>.ExpectedSingle(abilities.Count);
    }

    return abilities.SingleOrDefault().Value;
  }
}
