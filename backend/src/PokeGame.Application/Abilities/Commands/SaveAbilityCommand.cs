using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

internal record SaveAbilityCommand(Ability Ability) : IRequest;

internal class SaveAbilityCommandHandler : IRequestHandler<SaveAbilityCommand>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public SaveAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task Handle(SaveAbilityCommand command, CancellationToken cancellationToken)
  {
    Ability ability = command.Ability;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in ability.Changes)
    {
      if (change is Ability.CreatedEvent || change is Ability.UpdatedEvent updatedEvent && updatedEvent.UniqueName != null)
      {
        hasUniqueNameChanged = true;
        break;
      }
    }

    if (hasUniqueNameChanged)
    {
      AbilityId? abilityId = await _abilityQuerier.FindIdAsync(ability.UniqueName, cancellationToken);
      if (abilityId != null && abilityId != ability.Id)
      {
        throw new UniqueNameAlreadyUsedException(ability, abilityId.Value);
      }
    }

    await _abilityRepository.SaveAsync(ability, cancellationToken);
  }
}
