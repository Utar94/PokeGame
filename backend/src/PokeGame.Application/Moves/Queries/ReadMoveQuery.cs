using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Queries;

public record ReadMoveQuery(Guid? Id, string? UniqueName) : Activity, IRequest<Move?>;
