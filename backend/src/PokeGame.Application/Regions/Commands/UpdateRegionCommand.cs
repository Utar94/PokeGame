using FluentValidation;
using MediatR;
using PokeGame.Application.Regions.Validators;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

public record UpdateRegionCommand(Guid Id, UpdateRegionPayload Payload) : Activity, IRequest<RegionModel?>;

internal class UpdateRegionCommandHandler : IRequestHandler<UpdateRegionCommand, RegionModel?>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly IRegionRepository _regionRepository;
  private readonly ISender _sender;

  public UpdateRegionCommandHandler(IRegionQuerier regionQuerier, IRegionRepository regionRepository, ISender sender)
  {
    _regionQuerier = regionQuerier;
    _regionRepository = regionRepository;
    _sender = sender;
  }

  public async Task<RegionModel?> Handle(UpdateRegionCommand command, CancellationToken cancellationToken)
  {
    UpdateRegionPayload payload = command.Payload;
    new UpdateRegionValidator().ValidateAndThrow(payload);

    RegionId id = new(command.Id);
    Region? region = await _regionRepository.LoadAsync(id, cancellationToken);
    if (region == null)
    {
      return null;
    }

    UniqueName? uniqueName = UniqueName.TryCreate(payload.UniqueName);
    if (uniqueName != null)
    {
      region.UniqueName = uniqueName;
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

    region.Update(command.GetUserId());
    await _sender.Send(new SaveRegionCommand(region), cancellationToken);

    return await _regionQuerier.ReadAsync(region, cancellationToken);
  }
}
