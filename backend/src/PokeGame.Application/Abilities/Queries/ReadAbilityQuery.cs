using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

public record ReadAbilityQuery(Guid Id) : Activity, IRequest<AbilityModel?>;

internal class ReadAbilityQueryHandler : IRequestHandler<ReadAbilityQuery, AbilityModel?>
{
  private readonly IAbilityQuerier _abilityQuerier;

  public ReadAbilityQueryHandler(IAbilityQuerier abilityQuerier)
  {
    _abilityQuerier = abilityQuerier;
  }

  public async Task<AbilityModel?> Handle(ReadAbilityQuery query, CancellationToken cancellationToken)
  {
    return await _abilityQuerier.ReadAsync(query.Id, cancellationToken);
  }
}
