using MediatR;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Mutations
{
  internal class DeleteTrainerMutationHandler : IRequestHandler<DeleteTrainerMutation>
  {
    private readonly IRepository _repository;

    public DeleteTrainerMutationHandler(IRepository repository)
    {
      _repository = repository;
    }

    public async Task<Unit> Handle(DeleteTrainerMutation request, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync<Trainer>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.Id);

      trainer.Delete();

      await _repository.SaveAsync(trainer, cancellationToken);

      return Unit.Value;
    }
  }
}
