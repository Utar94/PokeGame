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
      CreateMap<MoveEntity, MoveModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
