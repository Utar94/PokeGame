using FluentValidation;
using MediatR;
using PokeGame.Application.Speciez.Models;
using PokeGame.Application.Speciez.Validators;
using PokeGame.Domain;
using PokeGame.Domain.Speciez;

namespace PokeGame.Application.Speciez.Commands;

public record UpdateSpeciesCommand(Guid Id, UpdateSpeciesPayload Payload) : IRequest<SpeciesModel?>;

internal class UpdateSpeciesCommandHandler : IRequestHandler<UpdateSpeciesCommand, SpeciesModel?>
{
  private readonly IApplicationContext _applicationContext;
  private readonly ISpeciesManager _speciesManager;
  private readonly ISpeciesQuerier _speciesQuerier;
  private readonly ISpeciesRepository _speciesRepository;

  public UpdateSpeciesCommandHandler(
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

  public async Task<SpeciesModel?> Handle(UpdateSpeciesCommand command, CancellationToken cancellationToken)
  {
    UpdateSpeciesPayload payload = command.Payload;
    new UpdateSpeciesValidator().ValidateAndThrow(payload);

    SpeciesId speciesId = new(command.Id);
    Species? species = await _speciesRepository.LoadAsync(speciesId, cancellationToken);
    if (species == null)
    {
      return null;
    }

    if (!string.IsNullOrWhiteSpace(payload.UniqueName))
    {
      species.UniqueName = new UniqueName(payload.UniqueName);
    }
    if (payload.DisplayName != null)
    {
      species.DisplayName = DisplayName.TryCreate(payload.DisplayName.Value);
    }

    if (payload.GrowthRate.HasValue)
    {
      species.GrowthRate = payload.GrowthRate.Value;
    }
    if (payload.BaseFriendship.HasValue)
    {
      species.BaseFriendship = new Friendship(payload.BaseFriendship.Value);
    }
    if (payload.CatchRate.HasValue)
    {
      species.CatchRate = new CatchRate(payload.CatchRate.Value);
    }

    // TODO(fpion): RegionalNumbers

    if (payload.Link != null)
    {
      species.Link = Url.TryCreate(payload.Link.Value);
    }
    if (payload.Notes != null)
    {
      species.Notes = Notes.TryCreate(payload.Notes.Value);
    }

    species.Update(_applicationContext.GetActorId());
    await _speciesManager.SaveAsync(species, cancellationToken);

    return await _speciesQuerier.ReadAsync(species, cancellationToken);
  }
}
