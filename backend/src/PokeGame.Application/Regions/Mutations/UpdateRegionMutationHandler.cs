using FluentValidation;
using MediatR;
using PokeGame.Application.Regions.Models;
using PokeGame.Domain.Regions;

namespace PokeGame.Application.Regions.Mutations
{
  internal class UpdateRegionMutationHandler : IRequestHandler<UpdateRegionMutation, RegionModel>
  {
    private readonly IRegionQuerier _querier;
    private readonly IRepository _repository;
    private readonly IValidator<Region> _validator;

    public UpdateRegionMutationHandler(
      IRegionQuerier querier,
      IRepository repository,
      IValidator<Region> validator
    )
    {
      _querier = querier;
      _repository = repository;
      _validator = validator;
    }

    public async Task<RegionModel> Handle(UpdateRegionMutation request, CancellationToken cancellationToken)
    {
      Region region = await _repository.LoadAsync<Region>(request.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Region>(request.Id);

      region.Update(request.Payload);
      _validator.ValidateAndThrow(region);

      await _repository.SaveAsync(region, cancellationToken);

      return await _querier.GetAsync(region.Id, cancellationToken)
        ?? throw new EntityNotFoundException<Region>(region.Id);
    }
  }
}
