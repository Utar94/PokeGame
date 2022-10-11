using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Application.Trainers.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class TrainerProfile : Profile
  {
    public TrainerProfile()
    {
      CreateMap<TrainerEntity, TrainerModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
