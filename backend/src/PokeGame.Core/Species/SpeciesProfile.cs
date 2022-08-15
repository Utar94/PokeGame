using AutoMapper;
using PokeGame.Core.Models;
using PokeGame.Core.Species.Models;

namespace PokeGame.Core.Species
{
  internal class SpeciesProfile : Profile
  {
    public SpeciesProfile()
    {
      CreateMap<Species, SpeciesModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<SpeciesModel, SpeciesSummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
