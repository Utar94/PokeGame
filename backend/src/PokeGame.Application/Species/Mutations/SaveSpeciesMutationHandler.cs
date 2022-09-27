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
    {
      if (payload.RegionalNumbers?.Any() == true)
      {
        IEnumerable<Region> regions = payload.RegionalNumbers.Select(x => x.Region).Distinct();
        Dictionary<Region, HashSet<int>> regionalNumbers = await Querier.GetRegionalNumbersAsync(regions, cancellationToken);

        var conflicts = new List<KeyValuePair<Region, int>>(capacity: payload.RegionalNumbers.Count());

        foreach (RegionalNumberPayload regionalNumber in payload.RegionalNumbers)
        {
          if (regionalNumbers.TryGetValue(regionalNumber.Region, out HashSet<int>? numbers)
            && numbers.Contains(regionalNumber.Number))
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
