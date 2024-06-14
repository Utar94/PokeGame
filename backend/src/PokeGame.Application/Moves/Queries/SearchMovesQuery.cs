using Logitar.Portal.Contracts.Search;
using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

public record SearchMovesQuery(SearchMovesPayload Payload) : Activity, IRequest<SearchResults<Move>>;
