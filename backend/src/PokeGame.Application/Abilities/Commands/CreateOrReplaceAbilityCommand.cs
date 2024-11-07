using FluentValidation;
using MediatR;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record CreateOrReplaceAbilityResult(AbilityModel? Ability = null, bool Created = false);

public record CreateOrReplaceAbilityCommand(Guid? Id, CreateOrReplaceAbilityPayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceAbilityResult>;

internal class CreateOrReplaceAbilityCommandHandler : IRequestHandler<CreateOrReplaceAbilityCommand, CreateOrReplaceAbilityResult>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public CreateOrReplaceAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task<CreateOrReplaceAbilityResult> Handle(CreateOrReplaceAbilityCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceAbilityPayload payload = command.Payload;
    new CreateOrReplaceAbilityValidator().ValidateAndThrow(payload);

    AbilityId? id = command.Id.HasValue ? new(command.Id.Value) : null;
    Ability? ability = null;
    if (id.HasValue)
    {
      ability = await _abilityRepository.LoadAsync(id.Value, cancellationToken);
    }

    UserId userId = command.GetUserId();
    bool created = false;
    if (ability == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceAbilityResult();
      }

      ability = new Ability(new UniqueName(payload.UniqueName), userId, id);
      created = true;
    }

    Ability reference = (command.Version.HasValue
      ? await _abilityRepository.LoadAsync(ability.Id, command.Version.Value, cancellationToken)
      : null) ?? ability;

    UniqueName uniqueName = new(payload.UniqueName);
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

    ability.Update(userId);
    await _abilityRepository.SaveAsync(ability, cancellationToken);

    AbilityModel model = await _abilityQuerier.ReadAsync(ability, cancellationToken);
    return new CreateOrReplaceAbilityResult(model, created);
  }
}
