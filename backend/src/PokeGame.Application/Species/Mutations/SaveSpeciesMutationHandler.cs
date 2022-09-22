using PokeGame.Domain.Abilities;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  internal abstract class SaveSpeciesMutationHandler
  {
    protected SaveSpeciesMutationHandler(IRepository repository)
    {
      Repository = repository;
    }

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
  }
}
