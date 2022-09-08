using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Trainers;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Trainers
{
  internal class SynchronizeTrainer
  {
    private readonly ReadContext _readContext;
    private readonly IRepository<Trainer> _repository;

    public SynchronizeTrainer(ReadContext readContext, IRepository<Trainer> repository)
    {
      _readContext = readContext;
      _repository = repository;
    }

    public async Task<TrainerEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      TrainerEntity? entity = await _readContext.Trainers
        .Include(x => x.Inventory).ThenInclude(x => x.Item)
        .Include(x => x.Pokedex).ThenInclude(x => x.Species)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Trainer? trainer = await _repository.LoadAsync(id, version, cancellationToken);
      if (trainer != null)
      {
        if (entity == null)
        {
          entity = new TrainerEntity { Id = id };
          _readContext.Trainers.Add(entity);
        }

        entity.Synchronize(trainer);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
