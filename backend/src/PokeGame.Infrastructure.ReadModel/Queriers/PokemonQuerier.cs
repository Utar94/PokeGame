using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Pokemon;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Queriers
{
  internal class PokemonQuerier : IPokemonQuerier
  {
    private readonly DbSet<PokemonEntity> _pokemon;
    private readonly IMapper _mapper;

    public PokemonQuerier(IMapper mapper, ReadContext readContext)
    {
      _pokemon = readContext.Pokemon;
      _mapper = mapper;
    }

    protected IQueryable<PokemonEntity> BaseQuery => _pokemon.AsNoTracking()
      .Include(x => x.Ability)
      .Include(x => x.CurrentTrainer)
      .Include(x => x.HeldItem)
      .Include(x => x.Moves).ThenInclude(x => x.Move)
      .Include(x => x.OriginalTrainer)
      .Include(x => x.Position)
      .Include(x => x.Species);

    public async Task<PokemonModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      PokemonEntity? pokemon = await BaseQuery.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      return pokemon == null ? null : _mapper.Map<PokemonModel>(pokemon);
    }

    public async Task<PokemonModel?> GetAsync(Guid trainerId, PokemonPosition position, CancellationToken cancellationToken)
    {
      ArgumentNullException.ThrowIfNull(position);

      byte box = position.Box ?? 0;

      PokemonEntity? pokemon = await BaseQuery.SingleOrDefaultAsync(x
        => x.CurrentTrainer!.Id == trainerId
        && x.Position!.Position == position.Position
        && x.Position.Box == box, cancellationToken);

      return pokemon == null ? null : _mapper.Map<PokemonModel>(pokemon);
    }

    public async Task<IEnumerable<PokemonModel>> GetAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken)
    {
      PokemonEntity[] pokemon = await BaseQuery
        .Where(x => ids.Contains(x.Id))
        .ToArrayAsync(cancellationToken);

      return _mapper.Map<IEnumerable<PokemonModel>>(pokemon);
    }

    public async Task<ListModel<PokemonModel>> GetPagedAsync(PokemonGender? gender, byte? inBox, bool? inParty, bool? isWild, string? search, Guid? speciesId, Guid? trainerId,
      PokemonSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      IQueryable<PokemonEntity> query = BaseQuery;

      if (gender.HasValue)
      {
        query = query.Where(x => x.Gender == gender.Value);
      }
      if (inBox.HasValue)
      {
        query = query.Where(x => x.Position!.Box == inBox.Value);
      }
      if (inParty.HasValue)
      {
        query = inParty.Value
          ? query.Where(x => x.Position!.Box == 0)
          : query.Where(x => x.Position!.Box > 0);
      }
      if (isWild.HasValue)
      {
        query = query.Where(x => x.CurrentTrainerId.HasValue == !isWild.Value);
      }
      if (search != null)
      {
        foreach (string term in search.Split())
        {
          if (!string.IsNullOrEmpty(term))
          {
            string pattern = $"%{term}%";

            query = query.Where(x => EF.Functions.ILike(x.Species!.Name, pattern)
              || x.Surname != null && EF.Functions.ILike(x.Surname, pattern));
          }
        }
      }
      if (speciesId.HasValue)
      {
        query = query.Where(x => x.Species!.Id == speciesId.Value);
      }
      if (trainerId.HasValue)
      {
        query = query.Where(x => x.CurrentTrainer!.Id == trainerId.Value);
      }

      long total = await query.LongCountAsync(cancellationToken);

      if (sort.HasValue)
      {
        query = sort.Value switch
        {
          PokemonSort.Level => desc ? query.OrderByDescending(x => x.Level) : query.OrderBy(x => x.Level),
          PokemonSort.Name => desc ? query.OrderByDescending(x => x.Surname ?? x.Species!.Name) : query.OrderBy(x => x.Surname ?? x.Species!.Name),
          PokemonSort.Position => desc ? query.OrderByDescending(x => x.Position!.Position) : query.OrderBy(x => x.Position!.Position),
          PokemonSort.UpdatedAt => desc ? query.OrderByDescending(x => x.UpdatedAt ?? x.CreatedAt) : query.OrderBy(x => x.UpdatedAt ?? x.CreatedAt),
          _ => throw new ArgumentException($"The Pokemon sort '{sort}' is not valid.", nameof(sort)),
        };
      }
      query = query.ApplyPaging(index, count);

      PokemonEntity[] pokemon = await query.ToArrayAsync(cancellationToken);

      return new()
      {
        Items = _mapper.Map<IEnumerable<PokemonModel>>(pokemon),
        Total = total
      };
    }
  }
}
