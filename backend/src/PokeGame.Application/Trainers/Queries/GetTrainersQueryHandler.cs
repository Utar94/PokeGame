using MediatR;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;

namespace PokeGame.Application.Trainers.Queries
{
  internal class GetTrainersQueryHandler : IRequestHandler<GetTrainersQuery, ListModel<TrainerModel>>
  {
    private readonly ITrainerQuerier _querier;

    public GetTrainersQueryHandler(ITrainerQuerier querier)
    {
      _querier = querier;
    }

    public async Task<ListModel<TrainerModel>> Handle(GetTrainersQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetPagedAsync(request.Gender, request.RegionId, request.Search, request.UserId,
        request.Sort, request.Desc,
        request.Index, request.Count,
        cancellationToken);
    }
  }
}
