using MediatR;
using PokeGame.Application.Inventories.Models;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers;
using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Inventories.Queries
{
  internal class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, ListModel<InventoryModel>>
  {
    private readonly IInventoryQuerier _querier;
    private readonly ITrainerQuerier _trainerQuerier;

    public GetInventoryQueryHandler(IInventoryQuerier querier, ITrainerQuerier trainerQuerier)
    {
      _querier = querier;
      _trainerQuerier = trainerQuerier;
    }

    public async Task<ListModel<InventoryModel>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
      TrainerModel trainer = await _trainerQuerier.GetAsync(request.TrainerId, cancellationToken)
        ?? throw new EntityNotFoundException<Trainer>(request.TrainerId);

      return await _querier.GetPagedAsync(trainer.Id, request.Category, request.Search,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
