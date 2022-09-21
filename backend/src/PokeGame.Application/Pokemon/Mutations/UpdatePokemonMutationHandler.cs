using FluentValidation;
using MediatR;
using PokeGame.Application.Pokemon.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Pokemon.Payloads;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokemon.Mutations
{
  internal class UpdatePokemonMutationHandler : IRequestHandler<UpdatePokemonMutation, PokemonModel>
  {
    private readonly IPokemonQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Pokemon.Pokemon> _validator;

    public UpdatePokemonMutationHandler(
      IPokemonQuerier querier,
      IRepository repository,
      IValidator<Domain.Pokemon.Pokemon> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<PokemonModel> Handle(UpdatePokemonMutation request, CancellationToken cancellationToken)
    {
      Domain.Pokemon.Pokemon pokemon = await _repository.LoadAsync<Domain.Pokemon.Pokemon>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(request.Id);

      UpdatePokemonPayload payload = request.Payload;

      if (payload.HeldItemId.HasValue && await _repository.LoadAsync<Item>(payload.HeldItemId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Item>(payload.HeldItemId.Value, nameof(payload.HeldItemId));
      }

      if (payload.History != null && await _repository.LoadAsync<Trainer>(payload.History.TrainerId, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Trainer>(payload.History.TrainerId, $"{nameof(payload.History)}.{nameof(payload.History.TrainerId)}");
      }

      if (payload.OriginalTrainerId.HasValue && await _repository.LoadAsync<Trainer>(payload.OriginalTrainerId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Trainer>(payload.OriginalTrainerId.Value, nameof(payload.OriginalTrainerId));
      }

      pokemon.Update(payload);
      _validator.ValidateAndThrow(pokemon);

      await _repository.SaveAsync(pokemon, cancellationToken);

      return await _querier.GetAsync(pokemon.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Pokemon.Pokemon>(pokemon.Id);
    }
  }
}
