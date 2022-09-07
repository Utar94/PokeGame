using AutoMapper;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class AbilityProfile : Profile
  {
    public AbilityProfile()
    {
      CreateMap<AbilityEntity, AbilityModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
