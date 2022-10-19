using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Species;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class EvolvePokemonMutationHandler : IRequestHandler<EvolvePokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _trainerValidator;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public EvolvePokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Trainer> trainerValidator,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _trainerValidator = trainerValidator;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(EvolvePokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      if (pokemon.History == null)
      {
        throw new TrainerIsRequiredException(pokemon);
      }
      Trainer trainer = await _repository.LoadAsync<Trainer>(pokemon.History.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(pokemon.History.TrainerId);

      EvolvePokemonPayload payload = request.Payload;

      bool removeHeldItem = await ValidateCanEvolveAsync(payload, pokemon, trainer, cancellationToken);

      Domain.Species.Species evolvedSpecies = await _repository.LoadAsync<Domain.Species.Species>(payload.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(payload.SpeciesId, nameof(payload.SpeciesId));

      if (!evolvedSpecies.AbilityIds.Contains(payload.AbilityId))
      {
        throw new InvalidAbilityException(evolvedSpecies, payload.AbilityId);
      }

      pokemon.Evolve(request.Payload, evolvedSpecies, removeHeldItem);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      if (!trainer.Pokedex.TryGetValue(pokemon.SpeciesId, out PokedexEntry? entry) || !entry.HasCaught)
      {
        trainer.SavePokedex(pokemon.SpeciesId, hasCaught: true);
        _trainerValidator.ValidateAndThrow(trainer);

        await _repository.SaveAsync(trainer, cancellationToken);
      }

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }

    public async Task<bool> ValidateCanEvolveAsync(EvolvePokemonPayload payload, Domain.Pokemon.Pokemon pokemon, Trainer trainer, CancellationToken cancellationToken)
    {
      Domain.Species.Species evolvingSpecies = await _repository.LoadAsync<Domain.Species.Species>(pokemon.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(pokemon.SpeciesId);

      if (!evolvingSpecies.Evolutions.TryGetValue(payload.SpeciesId, out Evolution? evolution))
      {
        throw new PokemonCannotEvolveException(pokemon, payload.SpeciesId);
      }

      var errors = new List<string>(capacity: 8);
      bool removeHeldItem = false;

      if (evolution.Gender?.Equals(pokemon.Gender) == false)
      {
        errors.Add($"Gender: (Expected={evolution.Gender}, Actual={pokemon.Gender})");
      }
      if (evolution.HighFriendship && !pokemon.HasHighFriendshid)
      {
        errors.Add($"HighFriendship: (Friendship={pokemon.Friendship})");
      }
      if (evolution.ItemId.HasValue)
      {
        if (evolution.Method == EvolutionMethod.Item)
        {
          Item item = await _repository.LoadAsync<Item>(evolution.ItemId.Value, cancellationToken)
            ?? throw new EntityNotFoundException<Item>(evolution.ItemId.Value);

          try
          {
            trainer.RemoveItem(item, quantity: 1);
          }
          catch (InsufficientQuantityException)
          {
            errors.Add($"TrainerItem: (Expected={evolution.ItemId.Value})");
          }
        }
        else if (pokemon.HeldItemId != evolution.ItemId.Value)
        {
          errors.Add($"HeldItem: (Expected={evolution.ItemId.Value})");
        }
        else
        {
          removeHeldItem = true;
        }
      }
      if (evolution.Level > pokemon.Level)
      {
        errors.Add($"Level: (Expected={evolution.Level}+, Actual={pokemon.Level}");
      }
      if (evolution.Location != null && payload.Location != evolution.Location)
      {
        errors.Add($"Location: (Expected={evolution.Location}, Actual={payload.Location})");
      }
      if (evolution.MoveId.HasValue && !pokemon.Moves.Any(x => x.MoveId == evolution.MoveId.Value))
      {
        errors.Add($"MoveId: (Expected={evolution.MoveId.Value})");
      }
      if (evolution.Region?.Equals(payload.Region) == false)
      {
        errors.Add($"Region: (Expected={evolution.Region}, Actual={payload.Region})");
      }
      if (evolution.TimeOfDay?.Equals(payload.TimeOfDay) == false)
      {
        errors.Add($"Region: (TimeOfDay={evolution.TimeOfDay}, TimeOfDay={payload.TimeOfDay})");
      }

      if (errors.Any())
      {
        throw new PokemonCannotEvolveException(pokemon, payload.SpeciesId, errors);
      }

      if (trainer != null)
      {
        await _repository.SaveAsync(trainer, cancellationToken);
      }

      return removeHeldItem;
    }
  }
}
