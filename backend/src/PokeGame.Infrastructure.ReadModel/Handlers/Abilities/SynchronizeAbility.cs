using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Abilities;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Abilities
{
  internal class SynchronizeAbility
  {
    private readonly ReadContext _readContext;
    private readonly IRepository<Ability> _repository;

    public SynchronizeAbility(ReadContext readContext, IRepository<Ability> repository)
    {
      _readContext = readContext;
      _repository = repository;
    }

    public async Task<AbilityEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      AbilityEntity? entity = await _readContext.Abilities
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Ability? ability = await _repository.LoadAsync(id, version, cancellationToken);
      if (ability != null)
      {
        if (entity == null)
        {
          entity = new AbilityEntity { Id = id };
          _readContext.Abilities.Add(entity);
        }

        entity.Synchronize(ability);

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }

    public async Task<IEnumerable<AbilityEntity>> ExecuteAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
    {
      Dictionary<Guid, AbilityEntity> entities = await _readContext.Abilities
        .Where(x => ids.Contains(x.Id))
        .ToDictionaryAsync(x => x.Id, x => x, cancellationToken);

      IEnumerable<Ability> abilities = await _repository.LoadAsync(ids, cancellationToken);
      foreach (Ability ability in abilities)
      {
        if (entities.TryGetValue(ability.Id, out AbilityEntity? entity))
        {
          if (entity.Version >= ability.Version)
          {
            continue;
          }
        }
        else
        {
          entity = new AbilityEntity { Id = ability.Id };
          entities[entity.Id] = entity;
          _readContext.Abilities.Add(entity);
        }

        entity.Synchronize(ability);
      }

      await _readContext.SaveChangesAsync(cancellationToken);

      return entities.Values;
    }
  }
}
