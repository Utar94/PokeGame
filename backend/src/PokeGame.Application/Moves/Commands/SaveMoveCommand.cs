using MediatR;
using PokeGame.Domain.Moves;

namespace PokeGame.Application.Moves.Commands;

internal record SaveMoveCommand(MoveAggregate Move) : IRequest;
