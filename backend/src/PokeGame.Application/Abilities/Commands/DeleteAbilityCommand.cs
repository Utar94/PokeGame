using MediatR;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record DeleteAbilityCommand(Guid Id) : Activity, IRequest<AbilityModel?>;

internal class DeleteAbilityCommandHandler : IRequestHandler<DeleteAbilityCommand, AbilityModel?>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public DeleteAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task<AbilityModel?> Handle(DeleteAbilityCommand command, CancellationToken cancellationToken)
  {
    AbilityId id = new(command.Id);
    Ability? ability = await _abilityRepository.LoadAsync(id, cancellationToken);
    if (ability == null)
    {
      return null;
    }
    AbilityModel model = await _abilityQuerier.ReadAsync(ability, cancellationToken);

    ability.Delete(command.GetUserId());

    await _abilityRepository.SaveAsync(ability, cancellationToken);

    return model;
  }
}
