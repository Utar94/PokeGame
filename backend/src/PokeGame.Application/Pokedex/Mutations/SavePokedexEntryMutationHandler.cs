using MediatR;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokedex.Mutations
{
  internal class SavePokedexEntryMutationHandler : IRequestHandler<SavePokedexEntryMutation, PokedexModel>
  {
    private readonly IPokedexQuerier _querier;
    private readonly IRepository _repository;

    public SavePokedexEntryMutationHandler(IPokedexQuerier querier, IRepository repository)
    {
      _querier = querier;
      _repository = repository;
    }

    public async Task<PokedexModel> Handle(SavePokedexEntryMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);

      trainer.SavePokedex(request.SpeciesId, request.HasCaught);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(request.TrainerId, request.SpeciesId, cancellationToken)
        ?? throw new PokedexNotFoundException(request.TrainerId, request.SpeciesId);
    }
  }
}
