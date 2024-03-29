﻿using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Abilities;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Queriers
{
  internal class AbilityQuerier : IAbilityQuerier
  {
    private readonly DbSet<AbilityEntity> _abilities;
    private readonly IMappingService _mappingService;

    public AbilityQuerier(IMappingService mappingService, ReadContext readContext)
    {
      _abilities = readContext.Abilities;
      _mappingService = mappingService;
    }

    public async Task<AbilityModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      AbilityEntity? ability = await _abilities.AsNoTracking()
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return await _mappingService.MapAsync<AbilityModel>(ability, cancellationToken);
    }

    public async Task<ListModel<AbilityModel>> GetPagedAsync(string? search, Guid? speciesId,
      AbilitySort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<AbilityEntity> query = _abilities.AsNoTracking()
        .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Species);

      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Name, pattern));
          }
        }
      }
      if (speciesId.HasValue)
      {
        query = query.Where(x => x.SpeciesAbilities.Any(y => y.Species!.Id == speciesId.Value));
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          AbilitySort.Name => desc ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name),
          AbilitySort.UpdatedOn => desc ? query.OrderByDescending(x => x.UpdatedOn ?? x.CreatedOn) : query.OrderBy(x => x.UpdatedOn ?? x.CreatedOn),
          _ => throw new ArgumentException($"The ability sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      AbilityEntity[] abilities = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = await _mappingService.MapAsync<AbilityModel>(abilities, cancellationToken),
        Total = total
      };
    }
  }
}
