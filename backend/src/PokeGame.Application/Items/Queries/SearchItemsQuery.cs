using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Queries;

public record SearchItemsQuery(SearchItemsPayload Payload) : Activity, IRequest<SearchResults<Item>>;
