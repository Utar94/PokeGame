using MediatR;
using PokeGame.Contracts.Items;

namespace PokeGame.Application.Items.Queries;

public record ReadItemQuery(Guid? Id, string? UniqueName) : Activity, IRequest<Item?>;
