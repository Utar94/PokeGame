using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record CreateOrReplaceAbilityResult(AbilityModel? Ability = null, bool Created = false);

public record CreateOrReplaceAbilityCommand(Guid? Id, CreateOrReplaceAbilityPayload Payload, long? Version) : IRequest<CreateOrReplaceAbilityResult>;

internal class CreateOrReplaceAbilityCommandHandler : IRequestHandler<CreateOrReplaceAbilityCommand, CreateOrReplaceAbilityResult>
{
  private readonly IAbilityManager _abilityManager;
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;
  private readonly IApplicationContext _applicationContext;

  public CreateOrReplaceAbilityCommandHandler(
    IAbilityManager abilityManager,
    IAbilityQuerier abilityQuerier,
    IAbilityRepository abilityRepository,
    IApplicationContext applicationContext)
  {
    _abilityManager = abilityManager;
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
    _applicationContext = applicationContext;
  }

  public async Task<CreateOrReplaceAbilityResult> Handle(CreateOrReplaceAbilityCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityPayload payload = command.Payload;
    new CreateOrReplaceAbilityValidator().ValidateAndThrow(payload);

    AbilityId? abilityId = null;
    Ability? ability = null;
    if (command.Id.HasValue)
    {
      abilityId = new(command.Id.Value);
      ability = await _abilityRepository.LoadAsync(abilityId.Value, cancellationToken);
    }

    ActorId? actorId = _applicationContext.GetActorId();
    UniqueName uniqueName = new(payload.UniqueName);

    bool created = false;
    if (ability == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceAbilityResult();
      }

      ability = new(uniqueName, actorId, abilityId);
      created = true;
    }

    Ability reference = (command.Version.HasValue
      ? await _abilityRepository.LoadAsync(ability.Id, command.Version.Value, cancellationToken)
      : null) ?? ability;

    if (reference.UniqueName != uniqueName)
    {
      ability.UniqueName = uniqueName;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      ability.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      ability.Description = description;
    }

    Url? link = Url.TryCreate(payload.Link);
    if (reference.Link != link)
    {
      ability.Link = link;
    }
    Notes? notes = Notes.TryCreate(payload.Notes);
    if (reference.Notes != notes)
    {
      ability.Notes = notes;
    }

    ability.Update(actorId);
    await _abilityManager.SaveAsync(ability, cancellationToken);

    AbilityModel model = await _abilityQuerier.ReadAsync(ability, cancellationToken);
    return new CreateOrReplaceAbilityResult(model, created);
  }
}
