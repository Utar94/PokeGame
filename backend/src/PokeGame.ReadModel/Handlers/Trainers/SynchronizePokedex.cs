using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.ReadModel.Entities;
using PokeGame.ReadModel.Handlers.Species;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class SynchronizePokedex
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;
    private readonly SynchronizeSpecies _synchronizeSpecies;
    private readonly SynchronizeTrainer _synchronizeTrainer;

    public SynchronizePokedex(
      ReadContext readContext,
      IRepository repository,
      SynchronizeSpecies synchronizeSpecies,
      SynchronizeTrainer synchronizeTrainer
    )
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeSpecies = synchronizeSpecies;
      _synchronizeTrainer = synchronizeTrainer;
    }

    public async Task<PokedexEntity?> ExecuteAsync(Guid trainerId, Guid speciesId, int version, CancellationToken cancellationToken = default)
    {
      TrainerEntity? trainerEntity = await _synchronizeTrainer.ExecuteAsync(trainerId, version, cancellationToken);
      if (trainerEntity == null)
      {
        return null;
      }

      SpeciesEntity? species = await _readContext.Species.Include(x => x.RegionalSpecies)
        .SingleOrDefaultAsync(x => x.Id == speciesId, cancellationToken)
        ?? await _synchronizeSpecies.ExecuteAsync(speciesId, version: null, cancellationToken);
      if (species == null)
      {
        return null;
      }

      PokedexEntity entity = trainerEntity.Pokedex.SingleOrDefault(x => x.SpeciesId == species.Sid)
        ?? trainerEntity.AddPokedex(species);

      Trainer? trainer = await _repository.LoadAsync<Trainer>(trainerId, version, cancellationToken);
      if (trainer == null)
      {
        return null;
      }

      if (trainer.Pokedex.TryGetValue(species.Id, out PokedexEntry? entry))
      {
        entity.Synchronize(entry);
      }
      else
      {
        trainerEntity.Pokedex.Remove(entity);
      }

      int regionalCount = await _readContext.RegionalSpecies.AsNoTracking()
          .Where(x => x.Region!.Sid == trainerEntity.RegionId)
          .CountAsync(cancellationToken);

      trainerEntity.UpdatePokedex(regionalCount);

      await _readContext.SaveChangesAsync(cancellationToken);

      return entity;
    }
  }
}
