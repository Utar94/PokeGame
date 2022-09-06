using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class AggregateProfile : Profile
  {
    public AggregateProfile()
    {
      CreateMap<Entity, AggregateModel>();
    }
  }
}
