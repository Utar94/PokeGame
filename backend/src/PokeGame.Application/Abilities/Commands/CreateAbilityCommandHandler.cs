using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Abilities.Validators;
using PokeGame.Contracts.Abilities;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;

namespace PokeGame.Application.Abilities.Commands;

internal class CreateAbilityCommandHandler : IRequestHandler<CreateAbilityCommand, Ability>
{
  private readonly IAbilityQuerier _abilityQuerier;
  private readonly ISender _sender;

  public CreateAbilityCommandHandler(IAbilityQuerier abilityQuerier, ISender sender)
  {
    _abilityQuerier = abilityQuerier;
    _sender = sender;
  }

  public async Task<Ability> Handle(CreateAbilityCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = AbilityAggregate.UniqueNameSettings;

    CreateAbilityPayload payload = command.Payload;
    new CreateAbilityValidator(uniqueNameSettings).ValidateAndThrow(payload);

    AbilityAggregate ability = new(new UniqueNameUnit(uniqueNameSettings, payload.UniqueName), command.ActorId)
    {
      DisplayName = DisplayNameUnit.TryCreate(payload.DisplayName),
      Description = DescriptionUnit.TryCreate(payload.Description),
      Reference = ReferenceUnit.TryCreate(payload.Reference),
      Notes = NotesUnit.TryCreate(payload.Notes)
    };
    ability.Update(command.ActorId);

    await _sender.Send(new SaveAbilityCommand(ability), cancellationToken);

    return await _abilityQuerier.ReadAsync(ability, cancellationToken);
  }
}
