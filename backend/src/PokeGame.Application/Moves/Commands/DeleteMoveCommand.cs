using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Commands;

public record DeleteMoveCommand(Guid Id) : Activity, IRequest<Move?>;
