using FluentValidation;
using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Mutations
{
  internal class CreateSpeciesMutationHandler : SaveSpeciesMutationHandler, IRequestHandler<CreateSpeciesMutation, SpeciesModel>
  {
    private readonly IValidator<Domain.Species.Species> _validator;

    public CreateSpeciesMutationHandler(
      ISpeciesQuerier querier,
      IRepository repository,
      IValidator<Domain.Species.Species> validator
    ) : base(querier, repository)
    {
      _validator = validator;
    }

    public async Task<SpeciesModel> Handle(CreateSpeciesMutation request, CancellationToken cancellationToken)
    {
      if (await Querier.GetAsync(request.Payload.Number, cancellationToken) != null)
      {
        throw new SpeciesNumberAlreadyUsedException(request.Payload.Number, nameof(request.Payload.Number));
      }

      await ValidateAbilitiesAsync(request.Payload, cancellationToken);
      await ValidateRegionalNumbersAsync(request.Payload, cancellationToken);

      var species = new Domain.Species.Species(request.Payload);
      _validator.ValidateAndThrow(species);

      await Repository.SaveAsync(species, cancellationToken);

      return await Querier.GetAsync(species.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(species.Id);
    }
  }
}
