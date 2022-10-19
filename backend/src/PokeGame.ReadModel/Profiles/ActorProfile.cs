using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class ActorProfile : Profile
  {
    public ActorProfile()
    {
      CreateMap<UserEntity, ActorModel>()
        .ForMember(x => x.Name, x => x.MapFrom(y => y.FullName ?? y.Username))
        .ForMember(x => x.Type, x => x.MapFrom(y => ActorType.User));
    }
  }
}
