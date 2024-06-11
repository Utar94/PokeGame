using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

public record SearchAbilitiesQuery(SearchAbilitiesPayload Payload) : IRequest<SearchResults<Ability>>;
