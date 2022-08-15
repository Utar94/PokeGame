using AutoMapper;
using PokeGame.Core.Models;
using PokeGame.Core.Moves.Models;

namespace PokeGame.Core.Moves
{
  internal class MoveProfile : Profile
  {
    public MoveProfile()
    {
      CreateMap<Move, MoveModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<MoveModel, MoveSummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
