using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Application.Abilities.Models;

namespace PokeGame.Application.Abilities.Queries;

public record SearchAbilitiesQuery(SearchAbilitiesPayload Payload) : IRequest<SearchResults<AbilityModel>>;

internal class SearchAbilitiesQueryHandler : IRequestHandler<SearchAbilitiesQuery, SearchResults<AbilityModel>>
{
  private readonly IAbilityQuerier _abilityQuerier;

  public SearchAbilitiesQueryHandler(IAbilityQuerier abilityQuerier)
  {
    _abilityQuerier = abilityQuerier;
  }

  public async Task<SearchResults<AbilityModel>> Handle(SearchAbilitiesQuery query, CancellationToken cancellationToken)
  {
    return await _abilityQuerier.SearchAsync(query.Payload, cancellationToken);
  }
}
