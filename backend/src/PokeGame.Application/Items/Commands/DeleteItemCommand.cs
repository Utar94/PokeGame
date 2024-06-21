using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Commands;

public record DeleteItemCommand(Guid Id) : Activity, IRequest<Item?>;
