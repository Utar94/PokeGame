using FluentValidation;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;
using PokeGame.Domain.Trainers.Payloads;

namespace PokeGame.Application.Trainers
{
  internal class TrainerService : ITrainerService
  {
    private readonly ITrainerQuerier _querier;
    private readonly IRepository<Trainer> _repository;
    private readonly IValidator<Trainer> _validator;

    public TrainerService(
      ITrainerQuerier querier,
      IRepository<Trainer> repository,
      IValidator<Trainer> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<TrainerModel> CreateAsync(CreateTrainerPayload payload, CancellationToken cancellationToken)
    {
      var trainer = new Trainer(payload);
      _validator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainer.Id);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(id);

      trainer.Delete();

      await _repository.SaveAsync(trainer, cancellationToken);
    }

    public async Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(id, cancellationToken);
    }

    public async Task<ListModel<TrainerModel>> GetAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(gender, region, search, userId,
        sort, desc,
        index, count,
        cancellationToken);
    }

    public async Task<TrainerModel> UpdateAsync(Guid id, UpdateTrainerPayload payload, CancellationToken cancellationToken)
    {
      Trainer trainer = await _repository.LoadAsync(id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(id);

      trainer.Update(payload);
      _validator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _querier.GetAsync(trainer.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(trainer.Id);
    }
  }
}
