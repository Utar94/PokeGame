using MediatR;
using PokeGame.Application.Trainers.Models;

namespace PokeGame.Application.Trainers.Queries
{
  internal class GetTrainerQueryHandler : IRequestHandler<GetTrainerQuery, TrainerModel?>
  {
    private readonly ITrainerQuerier _querier;

    public GetTrainerQueryHandler(ITrainerQuerier querier)
    {
      _querier = querier;
    }

    public async Task<TrainerModel?> Handle(GetTrainerQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(request.Id, cancellationToken);
    }
  }
}
