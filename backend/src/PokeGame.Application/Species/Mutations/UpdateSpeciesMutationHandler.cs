using FluentValidation;
using MediatR;
using PokeGame.Application.Species.Models;

namespace PokeGame.Application.Species.Mutations
{
  internal class UpdateSpeciesMutationHandler : SaveSpeciesMutationHandler, IRequestHandler<UpdateSpeciesMutation, SpeciesModel>
  {
    private readonly IValidator<Domain.Species.Species> _validator;

    public UpdateSpeciesMutationHandler(
      ISpeciesQuerier querier,
      IRepository repository,
      IValidator<Domain.Species.Species> validator
    ) : base(querier, repository)
    {
      _validator = validator;
    }

    public async Task<SpeciesModel> Handle(UpdateSpeciesMutation request, CancellationToken cancellationToken)
    {
      Domain.Species.Species species = await Repository.LoadAsync<Domain.Species.Species>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(request.Id);

      await ValidateAbilitiesAsync(request.Payload, cancellationToken);
      await ValidateRegionalNumbersAsync(request.Payload, cancellationToken);

      species.Update(request.Payload);
      _validator.ValidateAndThrow(species);

      await Repository.SaveAsync(species, cancellationToken);

      return await Querier.GetAsync(species.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Domain.Species.Species>(species.Id);
    }
  }
}
