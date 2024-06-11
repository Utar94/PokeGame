using MediatR;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

internal record SaveAbilityCommand(AbilityAggregate Ability) : IRequest;
