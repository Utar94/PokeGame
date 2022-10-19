using FluentValidation;
using MediatR;
using PokeGame.Application.Species;
using PokeGame.Application.Trainers;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Pokedex.Mutations
{
  internal class SeenSpeciesMutationHandler : IRequestHandler<SeenSpeciesMutation>
  {
    private readonly IRepository _repository;
    private readonly IValidator<Trainer> _trainerValidator;

    public SeenSpeciesMutationHandler(IRepository repository, IValidator<Trainer> trainerValidator)
    {
      _repository = repository;
      _trainerValidator = trainerValidator;
    }

    public async Task<Unit> Handle(SeenSpeciesMutation request, CancellationToken cancellationToken)
    {
      if (request.Payload.SpeciesIds?.Any() == true && request.Payload.TrainerIds?.Any() == true)
      {
        IEnumerable<Guid> trainerIds = request.Payload.TrainerIds.Distinct();
        IEnumerable<Trainer> trainers = await _repository
          .LoadAsync<Trainer>(trainerIds, cancellationToken);

        IEnumerable<Guid> missingTrainers = trainerIds.Except(trainers.Select(x => x.Id)).Distinct();
        if (missingTrainers.Any())
        {
          throw new TrainersNotFoundException(missingTrainers);
        }

        IEnumerable<Guid> speciesIds = request.Payload.SpeciesIds.Distinct();
        IEnumerable<Domain.Species.Species> speciesList = await _repository
          .LoadAsync<Domain.Species.Species>(speciesIds, cancellationToken);

        IEnumerable<Guid> missingSpecies = speciesIds.Except(speciesList.Select(x => x.Id)).Distinct();
        if (missingSpecies.Any())
        {
          throw new SpeciesNotFoundException(missingSpecies);
        }

        foreach (Trainer trainer in trainers)
        {
          foreach (Domain.Species.Species species in speciesList)
          {
            if (!trainer.Pokedex.ContainsKey(species.Id))
            {
              trainer.SavePokedex(species.Id, hasCaught: false);
            }
          }
        }

        foreach (Trainer trainer in trainers)
        {
          _trainerValidator.ValidateAndThrow(trainer);
        }

        await _repository.SaveAsync(trainers, cancellationToken);
      }

      return Unit.Value;
    }
  }
}
