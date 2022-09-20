using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Species;
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

          foreach (AbilityEntity ability in abilities)
          {
            entity.Add(ability);
          }
        }

        entity.Evolutions.Clear();
        if (species.Evolutions.Any())
        {
          IEnumerable<Guid> speciesIds = species.Evolutions.Select(x => x.SpeciesId).Distinct();
          Dictionary<Guid, SpeciesEntity> speciesEntities = await _readContext.Species
            .Where(x => speciesIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

          IEnumerable<Guid> itemIds = species.Evolutions.Where(x => x.ItemId.HasValue).Select(x => x.ItemId!.Value).Distinct();
          Dictionary<Guid, ItemEntity> itemEntities = itemIds.Any() ? await _readContext.Items
            .Where(x => itemIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken) : new();

          IEnumerable<Guid> moveIds = species.Evolutions.Where(x => x.MoveId.HasValue).Select(x => x.MoveId!.Value).Distinct();
          Dictionary<Guid, MoveEntity> moveEntities = moveIds.Any() ? await _readContext.Moves
            .Where(x => moveIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, x => x, cancellationToken) : new();

          foreach (Evolution evolution in species.Evolutions)
          {
            ItemEntity? item = null;
            MoveEntity? move = null;
            if (speciesEntities.TryGetValue(evolution.SpeciesId, out SpeciesEntity? speciesEntity)
              && (!evolution.ItemId.HasValue || itemEntities.TryGetValue(evolution.ItemId.Value, out item))
              && (!evolution.MoveId.HasValue || moveEntities.TryGetValue(evolution.MoveId.Value, out move)))
            {
              entity.Add(speciesEntity, evolution, item, move);
            }
          }
        }

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
