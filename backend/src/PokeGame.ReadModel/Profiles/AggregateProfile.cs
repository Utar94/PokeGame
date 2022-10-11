using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class AggregateProfile : Profile
  {
    public AggregateProfile()
    {
      CreateMap<Entity, AggregateModel>();
    }
  }
}
