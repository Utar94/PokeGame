using MediatR;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

internal class DeleteAbilityCommandHandler : IRequestHandler<DeleteAbilityCommand, Ability?>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public DeleteAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task<Ability?> Handle(DeleteAbilityCommand command, CancellationToken cancellationToken)
  {
    AbilityId id = new(command.Id);
    AbilityAggregate? ability = await _abilityRepository.LoadAsync(id, cancellationToken);
    if (ability == null)
    {
      return null;
    }
    Ability result = await _abilityQuerier.ReadAsync(ability, cancellationToken);

    ability.Delete(command.ActorId);

    await _abilityRepository.SaveAsync(ability, cancellationToken);

    return result;
  }
}
