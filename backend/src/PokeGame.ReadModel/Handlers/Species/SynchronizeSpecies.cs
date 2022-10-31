using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.ReadModel.Entities;
using PokeGame.ReadModel.Handlers.Abilities;

namespace PokeGame.ReadModel.Handlers.Species
{
  internal class SynchronizeSpecies
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;
    private readonly SynchronizeAbility _synchronizeAbility;

    public SynchronizeSpecies(
      ReadContext readContext,
      IRepository repository,
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
        .Include(x => x.Evolutions).ThenInclude(x => x.EvolvedSpecies)
        .Include(x => x.Evolutions).ThenInclude(x => x.Item)
        .Include(x => x.Evolutions).ThenInclude(x => x.Move)
        .Include(x => x.RegionalSpecies)
        .Include(x => x.SpeciesAbilities)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Domain.Species.Species? species = await _repository.LoadAsync<Domain.Species.Species>(id, version, cancellationToken);
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

          foreach (AbilityEntity ability in abilities)
          {
            entity.Add(ability);
          }
        }

        IEnumerable<Guid> regionIds = species.RegionalNumbers.Select(x => x.Key);
        Dictionary<Guid, RegionEntity> regions = await _readContext.Regions
          .Where(x => regionIds.Contains(x.Id))
          .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

        Dictionary<int, RegionalSpeciesEntity> regionalSpecies = entity.RegionalSpecies.ToDictionary(x => x.RegionId, x => x);
        foreach (var (regionId, number) in species.RegionalNumbers)
        {
          if (!regions.TryGetValue(regionId, out RegionEntity? region))
          {
            return null;
          }

          if (!regionalSpecies.TryGetValue(region.Sid, out RegionalSpeciesEntity? regionalEntity))
          {
            regionalEntity = new(entity, region);
            regionalSpecies.Add(region.Sid, regionalEntity);

            entity.RegionalSpecies.Add(regionalEntity);
          }

          regionalEntity.Number = number;
        }

        foreach (RegionalSpeciesEntity regionalEntity in regionalSpecies.Values)
        {
          if (regionalEntity.Region == null || !species.RegionalNumbers.ContainsKey(regionalEntity.Region.Id))
          {
            entity.RegionalSpecies.Remove(regionalEntity);
          }
        }

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
