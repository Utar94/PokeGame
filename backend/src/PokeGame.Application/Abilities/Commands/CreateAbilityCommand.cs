using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record CreateAbilityCommand(CreateAbilityPayload Payload) : Activity, IRequest<Ability>;
