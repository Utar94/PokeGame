using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using PokeGame.Application.Speciez.Models;
using PokeGame.Application.Speciez.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez.Commands;

public record CreateOrReplaceSpeciesResult(SpeciesModel? Species = null, bool Created = false);

public record CreateOrReplaceSpeciesCommand(Guid? Id, CreateOrReplaceSpeciesPayload Payload, long? Version) : IRequest<CreateOrReplaceSpeciesResult>;

internal class CreateOrReplaceSpeciesCommandHandler : IRequestHandler<CreateOrReplaceSpeciesCommand, CreateOrReplaceSpeciesResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ISpeciesManager _speciesManager;
  private readonly ISpeciesQuerier _speciesQuerier;
  private readonly ISpeciesRepository _speciesRepository;

  public CreateOrReplaceSpeciesCommandHandler(
    IApplicationContext applicationContext,
    ISpeciesManager speciesManager,
    ISpeciesQuerier speciesQuerier,
    ISpeciesRepository speciesRepository)
  {
    _applicationContext = applicationContext;
    _speciesManager = speciesManager;
    _speciesQuerier = speciesQuerier;
    _speciesRepository = speciesRepository;
  }

  public async Task<CreateOrReplaceSpeciesResult> Handle(CreateOrReplaceSpeciesCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceSpeciesPayload payload = command.Payload;
    new CreateOrReplaceSpeciesValidator().ValidateAndThrow(payload);

    SpeciesId? speciesId = null;
    Species? species = null;
    if (command.Id.HasValue)
    {
      speciesId = new(command.Id.Value);
      species = await _speciesRepository.LoadAsync(speciesId.Value, cancellationToken);
    }

    ActorId? actorId = _applicationContext.GetActorId();
    UniqueName uniqueName = new(payload.UniqueName);
    Friendship baseFriendship = new(payload.BaseFriendship);
    CatchRate catchRate = new(payload.CatchRate);

    bool created = false;
    if (species == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceSpeciesResult();
      }

      species = new(payload.Number, payload.Category, uniqueName, payload.GrowthRate, baseFriendship, catchRate, actorId, speciesId);
      created = true;
    }

    Species reference = (command.Version.HasValue
      ? await _speciesRepository.LoadAsync(species.Id, command.Version.Value, cancellationToken)
      : null) ?? species;

    if (reference.UniqueName != uniqueName)
    {
      species.UniqueName = uniqueName;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      species.DisplayName = displayName;
    }

    if (reference.GrowthRate != payload.GrowthRate)
    {
      species.GrowthRate = payload.GrowthRate;
    }
    if (reference.BaseFriendship != baseFriendship)
    {
      species.BaseFriendship = baseFriendship;
    }
    if (reference.CatchRate != catchRate)
    {
      species.CatchRate = catchRate;
    }

    // TODO(fpion): RegionalNumbers

    Url? link = Url.TryCreate(payload.Link);
    if (reference.Link != link)
    {
      species.Link = link;
    }
    Notes? notes = Notes.TryCreate(payload.Notes);
    if (reference.Notes != notes)
    {
      species.Notes = notes;
    }

    species.Update(actorId);
    await _speciesManager.SaveAsync(species, cancellationToken);

    SpeciesModel model = await _speciesQuerier.ReadAsync(species, cancellationToken);
    return new CreateOrReplaceSpeciesResult(model, created);
  }
}
