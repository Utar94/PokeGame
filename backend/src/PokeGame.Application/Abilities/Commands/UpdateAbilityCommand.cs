﻿using FluentValidation;
using MediatR;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

public record UpdateAbilityCommand(Guid Id, UpdateAbilityPayload Payload) : Activity, IRequest<AbilityModel?>;

internal class UpdateAbilityCommandHandler : IRequestHandler<UpdateAbilityCommand, AbilityModel?>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly IAbilityRepository _abilityRepository;

  public UpdateAbilityCommandHandler(IAbilityQuerier abilityQuerier, IAbilityRepository abilityRepository)
  {
    _abilityQuerier = abilityQuerier;
    _abilityRepository = abilityRepository;
  }

  public async Task<AbilityModel?> Handle(UpdateAbilityCommand command, CancellationToken cancellationToken)
  {
    UpdateAbilityPayload payload = command.Payload;
    new UpdateAbilityValidator().ValidateAndThrow(payload);

    AbilityId id = new(command.Id);
    Ability? ability = await _abilityRepository.LoadAsync(id, cancellationToken);
    if (ability == null)
    {
      return null;
    }

    if (payload.Kind != null)
    {
      ability.Kind = payload.Kind.Value;
    }

    Name? name = Name.TryCreate(payload.Name);
    if (name != null)
    {
      ability.Name = name;
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

    ability.Update(command.GetActorId());
    await _abilityRepository.SaveAsync(ability, cancellationToken);

    return await _abilityQuerier.ReadAsync(ability, cancellationToken);
  }
}