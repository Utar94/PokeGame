using AutoMapper;

namespace PokeGame.Core.Models
{
  internal class AggregateProfile : Profile
  {
    public AggregateProfile()
    {
      CreateMap<Aggregate, AggregateModel>();
      CreateMap<Aggregate, AggregateSummary>()
        .ForMember(x => x.UpdatedAt, x => x.MapFrom(y => y.UpdatedAt ?? y.CreatedAt));
      CreateMap<AggregateModel, AggregateSummary>()
        .ForMember(x => x.UpdatedAt, x => x.MapFrom(y => y.UpdatedAt ?? y.CreatedAt));
    }
  }
}
