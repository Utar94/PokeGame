using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.Infrastructure.ReadModel.Entities;
using PokeGame.Infrastructure.ReadModel.Handlers.Species;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class SynchronizePokedex
  {
    private readonly ReadContext _readContext;
    private readonly SynchronizeSpecies _synchronizeSpecies;
    private readonly SynchronizeTrainer _synchronizeTrainer;
    private readonly IRepository<Trainer> _trainerRepository;

    public SynchronizePokedex(
      ReadContext readContext,
      SynchronizeSpecies synchronizeSpecies,
      SynchronizeTrainer synchronizeTrainer,
      IRepository<Trainer> trainerRepository
    )
    {
      _readContext = readContext;
      _synchronizeSpecies = synchronizeSpecies;
      _synchronizeTrainer = synchronizeTrainer;
      _trainerRepository = trainerRepository;
    }

    public async Task<PokedexEntity?> ExecuteAsync(Guid trainerId, Guid speciesId, int version, CancellationToken cancellationToken = default)
    {
      TrainerEntity? trainerEntity = await _synchronizeTrainer.ExecuteAsync(trainerId, version, cancellationToken);
      if (trainerEntity == null)
      {
        return null;
      }

      SpeciesEntity? species = await _readContext.Species.SingleOrDefaultAsync(x => x.Id == speciesId, cancellationToken)
        ?? await _synchronizeSpecies.ExecuteAsync(speciesId, version: null, cancellationToken);
      if (species == null)
      {
        return null;
      }

      PokedexEntity entity = trainerEntity.Pokedex.SingleOrDefault(x => x.SpeciesId == species.Sid)
        ?? trainerEntity.AddPokedex(species);

      Trainer? trainer = await _trainerRepository.LoadAsync(trainerId, version, cancellationToken);
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

      await _readContext.SaveChangesAsync(cancellationToken);

      return entity;
    }
  }
}
