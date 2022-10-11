using AutoMapper;
using PokeGame.Application.Abilities.Models;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
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
