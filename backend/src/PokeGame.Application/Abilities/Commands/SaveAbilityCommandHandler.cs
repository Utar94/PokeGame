using Logitar.EventSourcing;
using MediatR;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.Application.Abilities.Commands;

internal class SaveAbilityCommandHandler : IRequestHandler<SaveAbilityCommand>
{
  private readonly IAbilityRepository _abilityRepository;

  public SaveAbilityCommandHandler(IAbilityRepository abilityRepository)
  {
    _abilityRepository = abilityRepository;
  }

  public async Task Handle(SaveAbilityCommand command, CancellationToken cancellationToken)
  {
    AbilityAggregate ability = command.Ability;

    bool hasUniqueNameChanged = false;
    foreach (DomainEvent change in ability.Changes)
    {
      if (change is AbilityCreatedEvent || (change is AbilityUpdatedEvent updated && updated.UniqueName != null))
      {
        hasUniqueNameChanged = true;
      }
    }

    if (hasUniqueNameChanged)
    {
      AbilityAggregate? other = await _abilityRepository.LoadAsync(ability.UniqueName, cancellationToken);
      if (other != null && !other.Equals(ability))
      {
        throw new UniqueNameAlreadyUsedException<AbilityAggregate>(ability.UniqueName, nameof(ability.UniqueName));
      }
    }

    await _abilityRepository.SaveAsync(ability, cancellationToken);
  }
}
