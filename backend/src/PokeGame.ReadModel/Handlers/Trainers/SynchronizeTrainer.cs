using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.ReadModel.Entities;
using PokeGame.ReadModel.Handlers.Regions;

namespace PokeGame.ReadModel.Handlers.Trainers
{
  internal class SynchronizeTrainer
  {
    private readonly ReadContext _readContext;
    private readonly IRepository _repository;
    private readonly SynchronizeRegion _synchronizeRegion;

    public SynchronizeTrainer(ReadContext readContext, IRepository repository, SynchronizeRegion synchronizeRegion)
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeRegion = synchronizeRegion;
    }

    public async Task<TrainerEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      TrainerEntity? entity = await _readContext.Trainers
        .Include(x => x.Inventory).ThenInclude(x => x.Item)
        .Include(x => x.Pokedex).ThenInclude(x => x.Species).ThenInclude(x => x!.RegionalSpecies)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Trainer? trainer = await _repository.LoadAsync<Trainer>(id, version, cancellationToken);
      if (trainer != null)
      {
        RegionEntity? region = await _readContext.Regions.SingleOrDefaultAsync(x => x.Id == trainer.RegionId, cancellationToken)
          ?? await _synchronizeRegion.ExecuteAsync(trainer.RegionId, version: null, cancellationToken);
        if (region == null)
        {
          return null;
        }

        UserEntity? user = null;
        if (trainer.UserId.HasValue)
        {
          user = await _readContext.Users.SingleOrDefaultAsync(x => x.Id == trainer.UserId.Value, cancellationToken);
          if (user == null)
          {
            return null;
          }
        }

        if (entity == null)
        {
          entity = new TrainerEntity { Id = id };
          _readContext.Trainers.Add(entity);
        }

        entity.Synchronize(trainer);
        entity.SetRegion(region);
        entity.SetUser(user);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
