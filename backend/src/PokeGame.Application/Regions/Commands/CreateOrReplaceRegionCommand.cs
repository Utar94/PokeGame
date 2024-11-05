﻿using FluentValidation;
using MediatR;
using PokeGame.Application.Regions.Validators;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

public record CreateOrReplaceRegionResult(RegionModel? Region = null, bool Created = false);

public record CreateOrReplaceRegionCommand(Guid? Id, CreateOrReplaceRegionPayload Payload, long? Version) : Activity, IRequest<CreateOrReplaceRegionResult>;

internal class CreateOrReplaceRegionCommandHandler : IRequestHandler<CreateOrReplaceRegionCommand, CreateOrReplaceRegionResult>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public CreateOrReplaceRegionCommandHandler(IRegionQuerier regionQuerier, IRegionRepository regionRepository)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
  }

  public async Task<CreateOrReplaceRegionResult> Handle(CreateOrReplaceRegionCommand command, CancellationToken cancellationToken)
  {
    CreateOrReplaceRegionPayload payload = command.Payload;
    new CreateOrReplaceRegionValidator().ValidateAndThrow(payload);

    RegionId? id = command.Id.HasValue ? new(command.Id.Value) : null;
    Region? region = null;
    if (id.HasValue)
    {
      region = await _regionRepository.LoadAsync(id.Value, cancellationToken);
    }

    UserId userId = command.GetUserId();
    bool created = false;
    if (region == null)
    {
      if (command.Version.HasValue)
      {
        return new CreateOrReplaceRegionResult();
      }

      region = new Region(new Name(payload.Name), userId, id);
      created = true;
    }

    Region reference = (command.Version.HasValue
      ? await _regionRepository.LoadAsync(region.Id, command.Version.Value, cancellationToken)
      : null) ?? region;

    Name name = new(payload.Name);
    if (reference.Name != name)
    {
      region.Name = name;
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

    region.Update(userId);
    await _regionRepository.SaveAsync(region, cancellationToken);

    RegionModel model = await _regionQuerier.ReadAsync(region, cancellationToken);
    return new CreateOrReplaceRegionResult(model, created);
  }
}