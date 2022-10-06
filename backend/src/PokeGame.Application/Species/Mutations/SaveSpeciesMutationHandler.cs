using PokeGame.Application.Species.Models;
using PokeGame.Domain;
using PokeGame.Domain.Abilities;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  internal abstract class SaveSpeciesMutationHandler
  {
    protected SaveSpeciesMutationHandler(ISpeciesQuerier querier, IRepository repository)
    {
      Querier = querier;
      Repository = repository;
    }

    protected ISpeciesQuerier Querier { get; }
    protected IRepository Repository { get; }

    protected async Task ValidateAbilitiesAsync(SaveSpeciesPayload payload, CancellationToken cancellationToken)
    {
      if (payload.AbilityIds?.Any() == true)
      {
        IEnumerable<Ability> abilities = await Repository.LoadAsync<Ability>(payload.AbilityIds, cancellationToken);
        IEnumerable<Guid> missingIds = payload.AbilityIds.Except(abilities.Select(x => x.Id)).Distinct();
        if (missingIds.Any())
        {
          throw new AbilitiesNotFoundException(missingIds);
        }
      }
    }

    protected async Task ValidateRegionalNumbersAsync(SaveSpeciesPayload payload, CancellationToken cancellationToken)
      => await ValidateRegionalNumbersAsync(species: null, payload, cancellationToken);
    protected async Task ValidateRegionalNumbersAsync(Domain.Species.Species? species, SaveSpeciesPayload payload, CancellationToken cancellationToken)
    {
      if (payload.RegionalNumbers?.Any() == true)
      {
        var conflicts = new List<KeyValuePair<Region, int>>(capacity: payload.RegionalNumbers.Count());

        foreach (RegionalNumberPayload regionalNumber in payload.RegionalNumbers)
        {
          SpeciesModel? conflict = await Querier.GetAsync(regionalNumber.Region, regionalNumber.Number, cancellationToken);
          if (conflict != null && conflict.Id != species?.Id)
          {
            conflicts.Add(new KeyValuePair<Region, int>(regionalNumber.Region, regionalNumber.Number));
          }
        }

        if (conflicts.Any())
        {
          throw new RegionalNumbersAlreadyUsedException(conflicts, nameof(payload.RegionalNumbers));
        }
      }
    }
  }
}
