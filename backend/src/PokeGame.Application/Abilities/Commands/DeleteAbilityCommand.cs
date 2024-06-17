using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record DeleteAbilityCommand(Guid Id) : Activity, IRequest<Ability?>;
