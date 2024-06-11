using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

internal class SearchAbilitiesQueryHandler : IRequestHandler<SearchAbilitiesQuery, SearchResults<Ability>>
{
  private readonly IAbilityQuerier _abilityQuerier;

  public SearchAbilitiesQueryHandler(IAbilityQuerier abilityQuerier)
  {
    _abilityQuerier = abilityQuerier;
  }

  public async Task<SearchResults<Ability>> Handle(SearchAbilitiesQuery query, CancellationToken cancellationToken)
  {
    return await _abilityQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
