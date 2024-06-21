using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Commands;

public record ReplaceItemCommand(Guid Id, ReplaceItemPayload Payload, long? Version) : Activity, IRequest<Item?>;
