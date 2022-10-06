using FluentValidation;
using MediatR;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Mutations
{
  internal class CreateTrainerMutationHandler : IRequestHandler<CreateTrainerMutation, TrainerModel>
  {
    private readonly ITrainerQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _validator;

    public CreateTrainerMutationHandler(
      ITrainerQuerier querier,
      IRepository repository,
      IValidator<Trainer> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<TrainerModel> Handle(CreateTrainerMutation request, CancellationToken cancellationToken)
    {
      var trainer = new Trainer(request.Payload);
      _validator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainer.Id);
    }
  }
}
