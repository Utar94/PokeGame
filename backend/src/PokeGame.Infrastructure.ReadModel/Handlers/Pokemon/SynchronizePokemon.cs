using Microsoft.EntityFrameworkCore;
using PokeGame.Application;
using PokeGame.Domain.Pokemon;
using PokeGame.Infrastructure.ReadModel.Entities;
using PokeGame.Infrastructure.ReadModel.Handlers.Items;
using PokeGame.Infrastructure.ReadModel.Handlers.Moves;
using PokeGame.Infrastructure.ReadModel.Handlers.Species;
using PokeGame.Infrastructure.ReadModel.Handlers.Trainers;

namespace PokeGame.Infrastructure.ReadModel.Handlers.Pokemon
{
  internal class SynchronizePokemon
  {
    private readonly ReadContext _readContext;
    private readonly IRepository<Domain.Pokemon.Pokemon> _repository;
    private readonly SynchronizeItem _synchronizeItem;
    private readonly SynchronizeMove _synchronizeMove;
    private readonly SynchronizeSpecies _synchronizeSpecies;
    private readonly SynchronizeTrainer _synchronizeTrainer;

    public SynchronizePokemon(
      ReadContext readContext,
      IRepository<Domain.Pokemon.Pokemon> repository,
      SynchronizeItem synchronizeItem,
      SynchronizeMove synchronizeMove,
      SynchronizeSpecies synchronizeSpecies,
      SynchronizeTrainer synchronizeTrainer
    )
    {
      _readContext = readContext;
      _repository = repository;
      _synchronizeItem = synchronizeItem;
      _synchronizeMove = synchronizeMove;
      _synchronizeSpecies = synchronizeSpecies;
      _synchronizeTrainer = synchronizeTrainer;
    }

    public async Task<PokemonEntity?> ExecuteAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
      PokemonEntity? entity = await _readContext.Pokemon
        .Include(x => x.Ability)
        .Include(x => x.CurrentTrainer)
        .Include(x => x.HeldItem)
        .Include(x => x.Moves).ThenInclude(x => x.Move)
        .Include(x => x.OriginalTrainer)
        .Include(x => x.Species)
        .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

      if (entity != null && version.HasValue && entity.Version >= version.Value)
      {
        return entity;
      }

      Domain.Pokemon.Pokemon? pokemon = await _repository.LoadAsync(id, version, cancellationToken);
      if (pokemon != null)
      {
        SpeciesEntity? species = await _readContext.Species
          .Include(x => x.SpeciesAbilities).ThenInclude(x => x.Ability)
          .SingleOrDefaultAsync(x => x.Id == pokemon.SpeciesId, cancellationToken)
          ?? await _synchronizeSpecies.ExecuteAsync(id, version: null, cancellationToken);
        if (species == null)
        {
          return null;
        }

        AbilityEntity? ability = species.SpeciesAbilities.SingleOrDefault(x => x.Ability?.Id == pokemon.AbilityId)?.Ability;
        if (ability == null)
        {
          return null;
        }

        ItemEntity? heldItem = null;
        if (pokemon.HeldItemId.HasValue)
        {
          heldItem = await _readContext.Items.SingleOrDefaultAsync(x => x.Id == pokemon.HeldItemId.Value, cancellationToken)
            ?? await _synchronizeItem.ExecuteAsync(pokemon.HeldItemId.Value, version: null, cancellationToken);
          if (heldItem == null)
          {
            return null;
          }
        }

        TrainerEntity? currentTrainer = null;
        if (pokemon.History != null)
        {
          currentTrainer = await _readContext.Trainers.SingleOrDefaultAsync(x => x.Id == pokemon.History.TrainerId, cancellationToken)
            ?? await _synchronizeTrainer.ExecuteAsync(pokemon.History.TrainerId, version: null, cancellationToken);
          if (currentTrainer == null)
          {
            return null;
          }
        }
        TrainerEntity? originalTrainer = null;
        if (pokemon.OriginalTrainerId.HasValue)
        {
          originalTrainer = await _readContext.Trainers.SingleOrDefaultAsync(x => x.Id == pokemon.OriginalTrainerId.Value, cancellationToken)
            ?? await _synchronizeTrainer.ExecuteAsync(pokemon.OriginalTrainerId.Value, version: null, cancellationToken);
          if (originalTrainer == null)
          {
            return null;
          }
        }

        if (entity == null)
        {
          entity = new PokemonEntity { Id = id };
          _readContext.Pokemon.Add(entity);
        }

        entity.Synchronize(pokemon);

        entity.Species = species;
        entity.SpeciesId = species.Sid;
        entity.Ability = ability;
        entity.AbilityId = ability.Sid;

        entity.Moves.Clear();
        if (pokemon.Moves.Any())
        {
          IEnumerable<Guid> moveIds = pokemon.Moves.Select(x => x.MoveId);
          List<MoveEntity> moves = await _readContext.Moves
            .Where(x => moveIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

          IEnumerable<Guid> missingIds = moveIds.Except(moves.Select(x => x.Id)).Distinct();
          if (missingIds.Any())
          {
            moves.AddRange(await _synchronizeMove.ExecuteAsync(missingIds, cancellationToken));
          }

          Dictionary<Guid, MoveEntity> moveIndex = moves.ToDictionary(x => x.Id, x => x);
          foreach (PokemonMove pokemonMove in pokemon.Moves)
          {
            if (moveIndex.TryGetValue(pokemonMove.MoveId, out MoveEntity? move))
            {
              entity.Moves.Add(new PokemonMoveEntity
              {
                Move = move,
                MoveId = move.Sid,
                Position = pokemonMove.Position,
                RemainingPowerPoints = pokemonMove.RemainingPowerPoints
              });
            }
          }
        }

        entity.HeldItem = heldItem;
        entity.HeldItemId = heldItem?.Sid;

        entity.CurrentTrainer = currentTrainer;
        entity.CurrentTrainerId = currentTrainer?.Sid;
        entity.OriginalTrainer = originalTrainer;
        entity.CurrentTrainerId = originalTrainer?.Sid;

        await _readContext.SaveChangesAsync(cancellationToken);
      }

      return entity;
    }
  }
}
