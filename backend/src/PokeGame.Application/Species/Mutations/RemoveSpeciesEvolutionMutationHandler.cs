using FluentValidation;
using MediatR;

namespace PokeGame.Application.Species.Mutations
{
  internal class RemoveSpeciesEvolutionMutationHandler : IRequestHandler<RemoveSpeciesEvolutionMutation>
  {
    private readonly IRepository _repository;
    private readonly IValidator<Domain.Species.Species> _validator;

    public RemoveSpeciesEvolutionMutationHandler(IRepository repository, IValidator<Domain.Species.Species> validator)
    {
      _repository = repository;
      _validator = validator;
    }

    public async Task<Unit> Handle(RemoveSpeciesEvolutionMutation request, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await _repository.LoadAsync<Domain.Species.Species>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.Id, nameof(request.Id));

      Domain.Species.Species evolvedSpecies = await _repository.LoadAsync<Domain.Species.Species>(request.SpeciesId, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.SpeciesId, nameof(request.SpeciesId));

      species.RemoveEvolution(evolvedSpecies);
      _validator.ValidateAndThrow(species);

      await _repository.SaveAsync(species, cancellationToken);

      return Unit.Value;
    }
  }
}
