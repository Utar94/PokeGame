using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class TrainerProfile : Profile
  {
    public TrainerProfile()
    {
      CreateMap<Trainer, TrainerModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
