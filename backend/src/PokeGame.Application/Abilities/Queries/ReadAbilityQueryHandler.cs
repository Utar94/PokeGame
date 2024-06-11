using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

internal class ReadAbilityQueryHandler : IRequestHandler<ReadAbilityQuery, Ability?>
{
  private readonly IAbilityQuerier _abilityQuerier;

  public ReadAbilityQueryHandler(IAbilityQuerier abilityQuerier)
  {
    _abilityQuerier = abilityQuerier;
  }

  public async Task<Ability?> Handle(ReadAbilityQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, Ability> abilities = new(capacity: 2);

    if (query.Id.HasValue)
    {
      Ability? ability = await _abilityQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (ability != null)
      {
        abilities[ability.Id] = ability;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      Ability? ability = await _abilityQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (ability != null)
      {
        abilities[ability.Id] = ability;
      }
    }

    if (abilities.Count > 1)
    {
      throw TooManyResultsException<Ability>.ExpectedSingle(abilities.Count);
    }

    return abilities.Values.SingleOrDefault();
  }
}
