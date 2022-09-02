using AutoMapper;
using PokeGame.Core.Models;
using PokeGame.Core.Trainers.Models;

namespace PokeGame.Core.Trainers
{
  internal class TrainerProfile : Profile
  {
    public TrainerProfile()
    {
      CreateMap<Trainer, TrainerModel>()
        .IncludeBase<Aggregate, AggregateModel>();
      CreateMap<TrainerModel, TrainerSummary>()
        .IncludeBase<AggregateModel, AggregateSummary>();
    }
  }
}
