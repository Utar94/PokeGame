using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Regions.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class RegionProfile : Profile
  {
    public RegionProfile()
    {
      CreateMap<RegionEntity, RegionModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
