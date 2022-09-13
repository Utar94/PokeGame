using FluentValidation;
using PokeGame.Application.Models;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon
{
  internal class PokemonService : IPokemonService
  {
    private readonly IRepository<Item> _itemRepository;
    private readonly IRepository<Move> _moveRepository;
    private readonly IPokemonQuerier _querier;
    private readonly IRepository<Domain.Pokemon.Pokemon> _repository;
    private readonly IRepository<Domain.Species.Species> _speciesRepository;
    private readonly IRepository<Trainer> _trainerRepository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public PokemonService(
      IRepository<Item> itemRepository,
      IRepository<Move> moveRepository,
      IPokemonQuerier querier,
      IRepository<Domain.Pokemon.Pokemon> repository,
      IRepository<Domain.Species.Species> speciesRepository,
      IRepository<Trainer> trainerRepository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _itemRepository = itemRepository;
      _moveRepository = moveRepository;
      _querier = querier;
      _repository = repository;
      _speciesRepository = speciesRepository;
      _trainerRepository = trainerRepository;
      _validator = validator;
    }

    public async Task<PokemonModel> CatchAsync(Guid id, CatchPokemonPayload payload, CancellationToken cancellationToken = default)
    {
      Trainer trainer = await _trainerRepository.LoadAsync(payload.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(payload.TrainerId, nameof(payload.TrainerId));

      PokemonPosition position = await FindFirstAvailablePositionAsync(trainer.Id, cancellationToken);

      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(id);

      if (payload.Heal != null)
      {
        pokemon.Heal(payload.Heal);
      }
      pokemon.Catch(payload.Location, trainer.Id, position, payload.Surname);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    public async Task<PokemonModel> CreateAsync(CreatePokemonPayload payload, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _speciesRepository.LoadAsync(payload.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(payload.SpeciesId, nameof(payload.SpeciesId));

      if (!species.AbilityIds.Contains(payload.AbilityId))
      {
        throw new InvalidAbilityException(species, payload.AbilityId);
      }

      if (payload.HeldItemId.HasValue && await _itemRepository.LoadAsync(payload.HeldItemId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Item>(payload.HeldItemId.Value, nameof(payload.HeldItemId));
      }

      if (payload.Moves != null)
      {
        IEnumerable<Guid> moveIds = payload.Moves.Select(x => x.MoveId);
        Dictionary<Guid, Move> moves = (await _moveRepository.LoadAsync(moveIds, cancellationToken))
          .ToDictionary(x => x.Id, x => x);

        var missingIds = new List<Guid>(capacity: moveIds.Count());

        foreach (PokemonMovePayload movePayload in payload.Moves)
        {
          if (!moves.TryGetValue(movePayload.MoveId, out Move? move))
          {
            missingIds.Add(movePayload.MoveId);

            continue;
          }
          else if (movePayload.RemainingPowerPoints > move.PowerPoints)
          {
            throw new RemainingPowerPointsExceededException(move, movePayload.RemainingPowerPoints);
          }
        }

        if (missingIds.Any())
        {
          throw new MovesNotFoundException(missingIds);
        }
      }

      if (payload.History != null && await _trainerRepository.LoadAsync(payload.History.TrainerId, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Trainer>(payload.History.TrainerId, $"{nameof(payload.History)}.{nameof(payload.History.TrainerId)}");
      }

      var pokemon = new Domain.Pokemon.Pokemon(payload, species);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(id);

      pokemon.Delete();

      await _repository.SaveAsync(pokemon, cancellationToken);
    }

    public async Task<PokemonModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<PokemonModel>> GetAsync(PokemonGender? gender, byte? inBox, bool? inParty, bool? isWild, string? search, Guid? speciesId, Guid? trainerId,
      PokemonSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(gender, inBox, inParty, isWild, search, speciesId, trainerId,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<PokemonModel> HealAsync(Guid id, HealPokemonPayload payload, CancellationToken cancellationToken = default)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(id);

      pokemon.Heal(payload);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    public async Task<PokemonModel> UpdateAsync(Guid id, UpdatePokemonPayload payload, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(id);

      pokemon.Update(payload);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    private async Task<PokemonPosition> FindFirstAvailablePositionAsync(Guid trainerId, CancellationToken cancellationToken)
    {
      ListModel<PokemonModel> trainerPokemon = await _querier
        .GetPagedAsync(trainerId: trainerId, cancellationToken: cancellationToken);

      HashSet<string> positions = trainerPokemon.Items.Where(x => x.Position.HasValue)
        .Select(x => x.Box.HasValue ? string.Join('_', x.Box.Value, x.Position!.Value) : x.Position!.Value.ToString()).ToHashSet();

      for (int position = 0; position <= 5; position++)
      {
        if (!positions.Contains(position.ToString()))
        {
          return new PokemonPosition((byte)position);
        }
      }

      for (int box = 0; box <= 31; box++)
      {
        for (int position = 0; position <= 29; position++)
        {
          if (!positions.Contains($"{box}_{position}"))
          {
            return new PokemonPosition((byte)position, (byte)box);
          }
        }
      }

      throw new NoAvailablePositionException(trainerId);
    }
  }
}
