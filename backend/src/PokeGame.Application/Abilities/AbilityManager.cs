using Logitar.EventSourcing;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Abilities.Events;

namespace PokeGame.Application.Abilities;

internal class AbilityManager : IAbilityManager
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public AbilityManager(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task SaveAsync(Ability ability, CancellationToken cancellationToken)
  {
    UniqueName? uniqueName = null;
    foreach (IEvent change in ability.Changes)
    {
      if (change is AbilityCreated created)
      {
        uniqueName = created.UniqueName;
      }
      else if (change is AbilityUpdated updated && updated.UniqueName != null)
      {
        uniqueName = updated.UniqueName;
      }
    }

    if (uniqueName != null)
    {
      AbilityId? conflictId = await _abilityQuerier.FindIdAsync(uniqueName, cancellationToken);
      if (conflictId.HasValue && !conflictId.Value.Equals(ability.Id))
      {
        throw new UniqueNameAlreadyUsedException(ability, conflictId.Value);
      }
    }

    await _abilityRepository.SaveAsync(ability, cancellationToken);
  }
}
