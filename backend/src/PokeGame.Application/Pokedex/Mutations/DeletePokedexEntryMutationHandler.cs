using MediatR;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokedex.Mutations
{
  internal class DeletePokedexEntryMutationHandler : IRequestHandler<DeletePokedexEntryMutation>
  {
    private readonly IRepository _repository;

    public DeletePokedexEntryMutationHandler(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeletePokedexEntryMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);

      trainer.RemovePokedex(request.SpeciesId);

      await _repository.SaveAsync(trainer, cancellationToken);

      return Unit.Value;
    }
  }
}
