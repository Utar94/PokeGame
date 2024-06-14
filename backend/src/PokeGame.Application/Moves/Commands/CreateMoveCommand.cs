using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Commands;

public record CreateMoveCommand(CreateMovePayload Payload) : Activity, IRequest<Move>;
