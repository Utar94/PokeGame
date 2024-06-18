using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Regions;

namespace PokeGame.Application.Regions.Queries;

public record SearchRegionsQuery(SearchRegionsPayload Payload) : Activity, IRequest<SearchResults<Region>>;
