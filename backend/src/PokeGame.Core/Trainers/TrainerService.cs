using FluentValidation;
using PokeGame.Core.Models;
using PokeGame.Core.Trainers.Models;
using PokeGame.Core.Trainers.Payloads;

namespace PokeGame.Core.Trainers
{
  internal class TrainerService : ITrainerService
  {
    private readonly IMappingService _mappingService;
    private readonly ITrainerQuerier _querier;
    private readonly IRepository<Trainer> _repository;
    private readonly IUserContext _userContext;
    private readonly IValidator<Trainer> _validator;

    public TrainerService(
      IMappingService mappingService,
      ITrainerQuerier querier,
      IRepository<Trainer> repository,
      IUserContext userContext,
      IValidator<Trainer> validator
    )
    {
      _mappingService = mappingService;
      _querier = querier;
      _repository = repository;
      _userContext = userContext;
      _validator = validator;
    }

    public async Task<TrainerModel> CreateAsync(CreateTrainerPayload payload, CancellationToken cancellationToken)
    {
      var trainer = new Trainer(payload, _userContext.Id);
      _validator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _mappingService.MapAsync<TrainerModel>(trainer, cancellationToken);
    }

    public async Task<TrainerModel> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      Trainer trainer = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(id);

      trainer.Delete(_userContext.Id);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _mappingService.MapAsync<TrainerModel>(trainer, cancellationToken);
    }

    public async Task<TrainerModel?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      Trainer? trainer = await _querier.GetAsync(id, readOnly: true, cancellationToken);
      if (trainer == null)
      {
        return null;
      }

      return await _mappingService.MapAsync<TrainerModel>(trainer, cancellationToken);
    }

    public async Task<ListModel<TrainerModel>> GetAsync(TrainerGender? gender, Region? region, string? search, Guid? userId,
      TrainerSort? sort, bool desc,
      int? index, int? count,
      CancellationToken cancellationToken)
    {
      PagedList<Trainer> trainers = await _querier.GetPagedAsync(gender, region, search, userId,
        sort, desc,
        index, count,
        readOnly: true, cancellationToken);

      return await _mappingService.MapAsync<Trainer, TrainerModel>(trainers, cancellationToken);
    }

    public async Task<TrainerModel> UpdateAsync(Guid id, UpdateTrainerPayload payload, CancellationToken cancellationToken)
    {
      Trainer trainer = await _querier.GetAsync(id, readOnly: false, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(id);

      trainer.Update(payload, _userContext.Id);
      _validator.ValidateAndThrow(trainer);

      await _repository.SaveAsync(trainer, cancellationToken);

      return await _mappingService.MapAsync<TrainerModel>(trainer, cancellationToken);
    }
  }
}
