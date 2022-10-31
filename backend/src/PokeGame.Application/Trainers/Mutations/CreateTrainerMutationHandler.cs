using FluentValidation;
using MediatR;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Mutations
{
  internal class CreateTrainerMutationHandler : SaveTrainerMutationHandler, IRequestHandler<CreateTrainerMutation, TrainerModel>
  {
    private readonly ITrainerQuerier _querier;
    private readonly IValidator<Trainer> _validator;

    public CreateTrainerMutationHandler(
      ITrainerQuerier querier,
      IRepository repository,
      IValidator<Trainer> validator
    ) : base(repository)
    {
      _querier = querier;
      _validator = validator;
    }

    public async Task<TrainerModel> Handle(CreateTrainerMutation request, CancellationToken cancellationToken)
    {
      await EnsureRegionExistsAsync(request.Payload, cancellationToken);

      var trainer = new Trainer(request.Payload);
      _validator.ValidateAndThrow(trainer);

      await Repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainer.Id);
    }
  }
}
