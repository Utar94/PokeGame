using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class WalkPokemonEggMutationHandler : IRequestHandler<WalkPokemonEggMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _trainerValidator;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public WalkPokemonEggMutationHandler(
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

    public async Task<PokemonModel> Handle(WalkPokemonEggMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      pokemon.WalkEgg(request.Payload.Steps);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      if (pokemon.RemainingHatchSteps == 0 && pokemon.History != null)
      {
        Trainer trainer = await _repository.LoadAsync<Trainer>(pokemon.History.TrainerId, cancellationToken)
          ?? throw new EntityNotFoundException<Trainer>(pokemon.History.TrainerId);

        if (!trainer.Pokedex.TryGetValue(pokemon.SpeciesId, out PokedexEntry? entry) || !entry.HasCaught)
        {
          trainer.SavePokedex(pokemon.SpeciesId, hasCaught: true);
          _trainerValidator.ValidateAndThrow(trainer);

          await _repository.SaveAsync(trainer, cancellationToken);
        }
      }

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
