using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Items;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Items
{
  internal class SynchronizeItem
  {
    private readonly ReadContext _readContext;
    private readonly IRepository<Item> _repository;

    public SynchronizeItem(ReadContext readContext, IRepository<Item> repository)
    {
      _readContext = readContext;
      _repository = repository;
    }

    public async Task<ItemEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      ItemEntity? entity = await _readContext.Items
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Item? item = await _repository.LoadAsync(id, version, cancellationToken);
      if (item != null)
      {
        if (entity == null)
        {
          entity = new ItemEntity { Id = id };
          _readContext.Items.Add(entity);
        }

        entity.Synchronize(item);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
