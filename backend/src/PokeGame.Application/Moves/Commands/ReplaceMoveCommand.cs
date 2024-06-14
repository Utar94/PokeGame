using MediatR;
using PokeGame.Contracts.Moves;

namespace PokeGame.Application.Moves.Commands;

public record ReplaceMoveCommand(Guid Id, ReplaceMovePayload Payload, long? Version) : Activity, IRequest<Move?>;
