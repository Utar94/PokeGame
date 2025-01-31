using MediatR;
using PokeGame.Application.Speciez.Models;

namespace PokeGame.Application.Speciez.Queries;

public record ReadSpeciesQuery(Guid? Id, string? UniqueName) : IRequest<SpeciesModel?>;

internal class ReadSpeciesQueryHandler : IRequestHandler<ReadSpeciesQuery, SpeciesModel?>
{
  private readonly ISpeciesQuerier _speciesQuerier;

  public ReadSpeciesQueryHandler(ISpeciesQuerier speciesQuerier)
  {
    _speciesQuerier = speciesQuerier;
  }

  public async Task<SpeciesModel?> Handle(ReadSpeciesQuery query, CancellationToken cancellationToken)
  {
    Dictionary<Guid, SpeciesModel> speciez = new(capacity: 2);

    if (query.Id.HasValue)
    {
      SpeciesModel? species = await _speciesQuerier.ReadAsync(query.Id.Value, cancellationToken);
      if (species != null)
      {
        speciez[species.Id] = species;
      }
    }

    if (!string.IsNullOrWhiteSpace(query.UniqueName))
    {
      SpeciesModel? species = await _speciesQuerier.ReadAsync(query.UniqueName, cancellationToken);
      if (species != null)
      {
        speciez[species.Id] = species;
      }
    }

    if (speciez.Count > 1)
    {
      throw TooManyResultsException<SpeciesModel>.ExpectedSingle(speciez.Count);
    }

    return speciez.SingleOrDefault().Value;
  }
}
