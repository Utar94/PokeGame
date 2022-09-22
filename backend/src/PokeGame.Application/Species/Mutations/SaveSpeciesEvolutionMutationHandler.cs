using FluentValidation;
using MediatR;
using PokeGame.Application.Species.Models;
using PokeGame.Domain.Items;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Species;
using PokeGame.Domain.Species.Payloads;

namespace PokeGame.Application.Species.Mutations
{
  internal class SaveSpeciesEvolutionMutationHandler : IRequestHandler<SaveSpeciesEvolutionMutation, EvolutionModel>
  {
    private readonly ISpeciesQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Species.Species> _validator;

    public SaveSpeciesEvolutionMutationHandler(
      ISpeciesQuerier querier,
      IRepository repository,
      IValidator<Domain.Species.Species> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<EvolutionModel> Handle(SaveSpeciesEvolutionMutation request, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _repository.LoadAsync<Domain.Species.Species>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.Id, nameof(request.Id));

      Domain.Species.Species evolvedSpecies = await _repository.LoadAsync<Domain.Species.Species>(request.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.SpeciesId, nameof(request.SpeciesId));

      SaveEvolutionPayload payload = request.Payload;

      if (payload.ItemId.HasValue && await _repository.LoadAsync<Item>(payload.ItemId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Item>(payload.ItemId.Value, nameof(payload.ItemId));
      }

      if (payload.MoveId.HasValue && await _repository.LoadAsync<Move>(payload.MoveId.Value, cancellationToken) == null)
      {
        throw new EntityNotFoundException<Move>(payload.MoveId.Value, nameof(payload.MoveId));
      }

      species.SaveEvolution(evolvedSpecies, payload);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return await _querier.GetEvolutionAsync(species.Id, evolvedSpecies.Id, cancellationToken)
        ?? throw new SpeciesEvolutionNotFoundException(species.Id, evolvedSpecies.Id);
    }
  }
}
