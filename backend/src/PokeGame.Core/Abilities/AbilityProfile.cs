using AutoMapper;
using PokeGame.Core.Abilities.Models;
using PokeGame.Core.Models;

namespace PokeGame.Core.Abilities
{
  internal class AbilityProfile : Profile
  {
    public AbilityProfile()
    {
      CreateMap<Ability, AbilityModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<AbilityModel, AbilitySummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
