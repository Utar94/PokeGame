using FluentValidation;
using MediatR;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Mutations
{
  internal class UpdateTrainerMutationHandler : SaveTrainerMutationHandler, IRequestHandler<UpdateTrainerMutation, TrainerModel>
  {
    private readonly ITrainerQuerier _querier;
    private readonly IValidator<Trainer> _validator;

    public UpdateTrainerMutationHandler(
      ITrainerQuerier querier,
      IRepository repository,
      IValidator<Trainer> validator
    ) : base(repository)
    {
      _querier = querier;
      _validator = validator;
    }

    public async Task<TrainerModel> Handle(UpdateTrainerMutation request, CancellationToken cancellationToken)
    {
      await EnsureRegionExistsAsync(request.Payload, cancellationToken);

      Trainer trainer = await Repository.LoadAsync<Trainer>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.Id);

      trainer.Update(request.Payload);
      _validator.ValidateAndThrow(trainer);

      await Repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainer.Id);
    }
  }
}
