using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Commands;

public record CreateItemCommand(CreateItemPayload Payload) : Activity, IRequest<Item>;
