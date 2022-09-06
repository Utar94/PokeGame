using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Moves.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class MoveProfile : Profile
  {
    public MoveProfile()
    {
      CreateMap<Move, MoveModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
