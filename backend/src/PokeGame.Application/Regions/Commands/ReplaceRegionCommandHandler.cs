using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Regions.Validators;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

internal class ReplaceRegionCommandHandler : IRequestHandler<ReplaceRegionCommand, Region?>
{
  private readonly IRegionRepository _regionRepository;
  private readonly IRegionQuerier _regionQuerier;
  private readonly ISender _sender;

  public ReplaceRegionCommandHandler(IRegionRepository regionRepository, IRegionQuerier regionQuerier, ISender sender)
  {
    _regionRepository = regionRepository;
    _regionQuerier = regionQuerier;
    _sender = sender;
  }

  public async Task<Region?> Handle(ReplaceRegionCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = RegionAggregate.UniqueNameSettings;

    ReplaceRegionPayload payload = command.Payload;
    new ReplaceRegionValidator(uniqueNameSettings).ValidateAndThrow(payload);

    RegionId id = new(command.Id);
    RegionAggregate? region = await _regionRepository.LoadAsync(id, cancellationToken);
    if (region == null)
    {
      return null;
    }
    RegionAggregate? reference = null;
    if (command.Version.HasValue)
    {
      reference = await _regionRepository.LoadAsync(region.Id, command.Version.Value, cancellationToken);
    }

    UniqueNameUnit uniqueName = new(uniqueNameSettings, payload.UniqueName);
    if (reference == null || uniqueName != reference.UniqueName)
    {
      region.UniqueName = uniqueName;
    }
    DisplayNameUnit? displayName = DisplayNameUnit.TryCreate(payload.DisplayName);
    if (reference == null || displayName != reference.DisplayName)
    {
      region.DisplayName = displayName;
    }
    DescriptionUnit? description = DescriptionUnit.TryCreate(payload.Description);
    if (reference == null || description != reference.Description)
    {
      region.Description = description;
    }

    UrlUnit? url = UrlUnit.TryCreate(payload.Reference);
    if (reference == null || url != reference.Reference)
    {
      region.Reference = url;
    }
    NotesUnit? notes = NotesUnit.TryCreate(payload.Notes);
    if (reference == null || notes != reference.Notes)
    {
      region.Notes = notes;
    }

    region.Update(command.ActorId);

    await _sender.Send(new SaveRegionCommand(region), cancellationToken);

    return await _regionQuerier.ReadAsync(region, cancellationToken);
  }
}
