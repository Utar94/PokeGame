using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Infrastructure.ReadModel.Entities;
using PokeGame.Infrastructure.ReadModel.Handlers.Abilities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Species
{
  internal class SynchronizeSpecies
  {
    private readonly ReadContext _readContext;
    private readonly IRepository<Domain.Species.Species> _repository;
    private readonly SynchronizeAbility _synchronizeAbility;

    public SynchronizeSpecies(
      ReadContext readContext,
      IRepository<Domain.Species.Species> repository,
      SynchronizeAbility synchronizeAbility
    )
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeAbility = synchronizeAbility;
    }

    public async Task<SpeciesEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      SpeciesEntity? entity = await _readContext.Species
        .Include(x => x.SpeciesAbilities)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Domain.Species.Species? species = await _repository.LoadAsync(id, version, cancellationToken);
      if (species != null)
      {
        if (entity == null)
        {
          entity = new SpeciesEntity { Id = id };
          _readContext.Species.Add(entity);
        }

        entity.Synchronize(species);

        entity.SpeciesAbilities.Clear();
        if (species.AbilityIds.Any())
        {
          List<AbilityEntity> abilities = await _readContext.Abilities
            .Where(x => species.AbilityIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

          IEnumerable<Guid> missingIds = species.AbilityIds.Except(abilities.Select(x => x.Id)).Distinct();
          if (missingIds.Any())
          {
            abilities.AddRange(await _synchronizeAbility.ExecuteAsync(missingIds, cancellationToken));
          }

          entity.SpeciesAbilities.AddRange(abilities.Select(ability => new SpeciesAbilityEntity
          {
            Ability = ability,
            AbilityId = ability.Sid
          }));
        }

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
