using FluentValidation;
using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Mutations
{
  internal class CreateSpeciesMutationHandler : SaveSpeciesMutationHandler, IRequestHandler<CreateSpeciesMutation, SpeciesModel>
  {
    private readonly ISpeciesQuerier _querier;
    private readonly IValidator<Domain.Species.Species> _validator;

    public CreateSpeciesMutationHandler(
      ISpeciesQuerier querier,
      IRepository repository,
      IValidator<Domain.Species.Species> validator
    ) : base(repository)
    {
      _querier = querier;
      _validator = validator;
    }

    public async Task<SpeciesModel> Handle(CreateSpeciesMutation request, CancellationToken cancellationToken)
    {
      await ValidateAbilitiesAsync(request.Payload, cancellationToken);

      var species = new Domain.Species.Species(request.Payload);
      _validator.ValidateAndThrow(species);

      await Repository.SaveAsync(species, cancellationToken);

      return await _querier.GetAsync(species.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(species.Id);
    }
  }
}
