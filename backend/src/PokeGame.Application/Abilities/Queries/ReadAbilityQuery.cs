using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Queries;

public record ReadAbilityQuery(Guid? Id, string? UniqueName) : IRequest<Ability?>;
