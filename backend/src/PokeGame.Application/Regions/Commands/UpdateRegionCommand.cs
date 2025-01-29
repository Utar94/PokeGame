using FluentValidation;
using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Application.Regions.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

public record UpdateRegionCommand(Guid Id, UpdateRegionPayload Payload) : IRequest<RegionModel?>;

internal class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, RegionModel?>
{
  private readonly IApplicationContext _applicationContext;
  private readonly IRegionManager _regionManager;
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;

  public UpdateRegionCommandHandler(
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

  public async Task<RegionModel?> Handle(UpdateRegionCommand command, CancellationToken cancellationToken)
  {
    UpdateRegionPayload payload = command.Payload;
    new UpdateRegionValidator().ValidateAndThrow(payload);

    RegionId regionId = new(command.Id);
    Region? region = await _regionRepository.LoadAsync(regionId, cancellationToken);
    if (region == null)
    {
      return null;
    }

    if (!string.IsNullOrWhiteSpace(payload.UniqueName))
    {
      region.UniqueName = new UniqueName(payload.UniqueName);
    }
    if (payload.DisplayName != null)
    {
      region.DisplayName = DisplayName.TryCreate(payload.DisplayName.Value);
    }
    if (payload.Description != null)
    {
      region.Description = Description.TryCreate(payload.Description.Value);
    }

    if (payload.Link != null)
    {
      region.Link = Url.TryCreate(payload.Link.Value);
    }
    if (payload.Notes != null)
    {
      region.Notes = Notes.TryCreate(payload.Notes.Value);
    }

    region.Update(_applicationContext.GetActorId());
    await _regionManager.SaveAsync(region, cancellationToken);

    return await _regionQuerier.ReadAsync(region, cancellationToken);
  }
}
