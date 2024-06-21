using MediatR;
using PokeGame.Domain.Items;

namespace PokeGame.Application.Items.Commands;

internal record SaveItemCommand(ItemAggregate Item) : IRequest;
