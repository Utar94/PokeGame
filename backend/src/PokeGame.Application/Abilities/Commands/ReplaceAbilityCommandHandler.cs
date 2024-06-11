using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

internal class ReplaceAbilityCommandHandler : IRequestHandler<ReplaceAbilityCommand, Ability?>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;
  private readonly ISender _sender;

  public ReplaceAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository, ISender sender)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
    _sender = sender;
  }

  public async Task<Ability?> Handle(ReplaceAbilityCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = AbilityAggregate.UniqueNameSettings;

    ReplaceAbilityPayload payload = command.Payload;
    new ReplaceAbilityValidator(uniqueNameSettings).ValidateAndThrow(payload);

    AbilityId id = new(command.Id);
    AbilityAggregate? ability = await _abilityRepository.LoadAsync(id, cancellationToken);
    if (ability == null)
    {
      return null;
    }
    AbilityAggregate? reference = null;
    if (command.Version.HasValue)
    {
      reference = await _abilityRepository.LoadAsync(ability.Id, command.Version.Value, cancellationToken);
    }

    UniqueNameUnit uniqueName = new(uniqueNameSettings, payload.UniqueName);
    if (reference == null || uniqueName != reference.UniqueName)
    {
      ability.UniqueName = uniqueName;
    }
    DisplayNameUnit? displayName = DisplayNameUnit.TryCreate(payload.DisplayName);
    if (reference == null || displayName != reference.DisplayName)
    {
      ability.DisplayName = displayName;
    }
    DescriptionUnit? description = DescriptionUnit.TryCreate(payload.Description);
    if (reference == null || description != reference.Description)
    {
      ability.Description = description;
    }

    UrlUnit? url = UrlUnit.TryCreate(payload.Reference);
    if (reference == null || url != reference.Reference)
    {
      ability.Reference = url;
    }
    NotesUnit? notes = NotesUnit.TryCreate(payload.Notes);
    if (reference == null || notes != reference.Notes)
    {
      ability.Notes = notes;
    }

    ability.Update(command.ActorId);

    await _sender.Send(new SaveAbilityCommand(ability), cancellationToken);

    return await _abilityQuerier.ReadAsync(ability, cancellationToken);
  }
}
