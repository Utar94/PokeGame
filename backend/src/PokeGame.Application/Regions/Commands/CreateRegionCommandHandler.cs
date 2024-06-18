using FluentValidation;
using Logitar.Identity.Contracts.Settings;
using Logitar.Identity.Domain.Shared;
using MediatR;
using PokeGame.Application.Regions.Validators;
using PokeGame.Contracts.Regions;
using PokeGame.Domain;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Commands;

internal class CreateRegionCommandHandler : IRequestHandler<CreateRegionCommand, Region>
{
  private readonly IRegionQuerier _regionQuerier;
  private readonly ISender _sender;

  public CreateRegionCommandHandler(IRegionQuerier regionQuerier, ISender sender)
  {
    _regionQuerier = regionQuerier;
    _sender = sender;
  }

  public async Task<Region> Handle(CreateRegionCommand command, CancellationToken cancellationToken)
  {
    IUniqueNameSettings uniqueNameSettings = RegionAggregate.UniqueNameSettings;

    CreateRegionPayload payload = command.Payload;
    new CreateRegionValidator(uniqueNameSettings).ValidateAndThrow(payload);

    RegionAggregate region = new(new UniqueNameUnit(uniqueNameSettings, payload.UniqueName), command.ActorId)
    {
      DisplayName = DisplayNameUnit.TryCreate(payload.DisplayName),
      Description = DescriptionUnit.TryCreate(payload.Description),
      Reference = UrlUnit.TryCreate(payload.Reference),
      Notes = NotesUnit.TryCreate(payload.Notes)
    };
    region.Update(command.ActorId);

    await _sender.Send(new SaveRegionCommand(region), cancellationToken);

    return await _regionQuerier.ReadAsync(region, cancellationToken);
  }
}
