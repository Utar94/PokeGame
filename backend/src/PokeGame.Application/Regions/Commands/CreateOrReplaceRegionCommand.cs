using FluentValidation;
using Logitar.EventSourcing;
using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Regions.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

public record CreateOrReplaceRegionResult(RegionModel? Region = null, bool Created = false);

public record CreateOrReplaceRegionCommand(Guid? Id, CreateOrReplaceRegionPayload Payload, long? Version) : IRequest<CreateOrReplaceRegionResult>;

internal class CreateOrReplaceRegionCommandHandler : IRequestHandler<CreateOrReplaceRegionCommand, CreateOrReplaceRegionResult>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IRegionManager _regionManager;
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public CreateOrReplaceRegionCommandHandler(
    IApplicationContext applicationContext,
    IRegionManager regionManager,
    IRegionQuerier regionQuerier,
    IRegionRepository regionRepository)
  {
    _applicationContext = applicationContext;
    _regionManager = regionManager;
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task<CreateOrReplaceRegionResult> Handle(CreateOrReplaceRegionCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionPayload payload = command.Payload;
    new CreateOrReplaceRegionValidator().ValidateAndThrow(payload);

    RegionId? regionId = null;
    Region? region = null;
    if (command.Id.HasValue)
    {
      regionId = new(command.Id.Value);
      region = await _regionRepository.LoadAsync(regionId.Value, cancellationToken);
    }

    ActorId? actorId = _applicationContext.GetActorId();
    UniqueName uniqueName = new(payload.UniqueName);

    bool created = false;
    if (region == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceRegionResult();
      }

      region = new(uniqueName, actorId, regionId);
      created = true;
    }

    Region reference = (command.Version.HasValue
      ? await _regionRepository.LoadAsync(region.Id, command.Version.Value, cancellationToken)
      : null) ?? region;

    if (reference.UniqueName != uniqueName)
    {
      region.UniqueName = uniqueName;
    }
    DisplayName? displayName = DisplayName.TryCreate(payload.DisplayName);
    if (reference.DisplayName != displayName)
    {
      region.DisplayName = displayName;
    }
    Description? description = Description.TryCreate(payload.Description);
    if (reference.Description != description)
    {
      region.Description = description;
    }

    Url? link = Url.TryCreate(payload.Link);
    if (reference.Link != link)
    {
      region.Link = link;
    }
    Notes? notes = Notes.TryCreate(payload.Notes);
    if (reference.Notes != notes)
    {
      region.Notes = notes;
    }

    region.Update(actorId);
    await _regionManager.SaveAsync(region, cancellationToken);

    RegionModel model = await _regionQuerier.ReadAsync(region, cancellationToken);
    return new CreateOrReplaceRegionResult(model, created);
  }
}
