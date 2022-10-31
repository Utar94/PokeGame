using FluentValidation;
using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Mutations
{
  internal class CreateRegionMutationHandler : IRequestHandler<CreateRegionMutation, RegionModel>
  {
    private readonly IRegionQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Region> _validator;

    public CreateRegionMutationHandler(
      IRegionQuerier querier,
      IRepository repository,
      IValidator<Region> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<RegionModel> Handle(CreateRegionMutation request, CancellationToken cancellationToken)
    {
      var region = new Region(request.Payload);
      _validator.ValidateAndThrow(region);

      await _repository.SaveAsync(region, cancellationToken);

      return await _querier.GetAsync(region.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Region>(region.Id);
    }
  }
}
