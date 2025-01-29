using FluentValidation;
using MediatR;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record UpdateAbilityCommand(Guid Id, UpdateAbilityPayload Payload) : IRequest<AbilityModel?>;

internal class UpdateAbilityCommandHandler : IRequestHandler<UpdateAbilityCommand, AbilityModel?>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IAbilityManager _abilityManager;
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public UpdateAbilityCommandHandler(
    IApplicationContext applicationContext,
    IAbilityManager abilityManager,
    IAbilityQuerier abilityQuerier,
    IAbilityRepository abilityRepository)
  {
    _applicationContext = applicationContext;
    _abilityManager = abilityManager;
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task<AbilityModel?> Handle(UpdateAbilityCommand command, CancellationToken cancellationToken)
  {
    UpdateAbilityPayload payload = command.Payload;
    new UpdateAbilityValidator().ValidateAndThrow(payload);

    AbilityId abilityId = new(command.Id);
    Ability? ability = await _abilityRepository.LoadAsync(abilityId, cancellationToken);
    if (ability == null)
    {
      return null;
    }

    if (!string.IsNullOrWhiteSpace(payload.UniqueName))
    {
      ability.UniqueName = new UniqueName(payload.UniqueName);
    }
    if (payload.DisplayName != null)
    {
      ability.DisplayName = DisplayName.TryCreate(payload.DisplayName.Value);
    }
    if (payload.Description != null)
    {
      ability.Description = Description.TryCreate(payload.Description.Value);
    }

    if (payload.Link != null)
    {
      ability.Link = Url.TryCreate(payload.Link.Value);
    }
    if (payload.Notes != null)
    {
      ability.Notes = Notes.TryCreate(payload.Notes.Value);
    }

    ability.Update(_applicationContext.GetActorId());
    await _abilityManager.SaveAsync(ability, cancellationToken);

    return await _abilityQuerier.ReadAsync(ability, cancellationToken);
  }
}
