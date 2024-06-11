using MediatR;
using PokeGame.Contracts.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record ReplaceAbilityCommand(Guid Id, ReplaceAbilityPayload Payload, long? Version) : Activity, IRequest<Ability?>;
