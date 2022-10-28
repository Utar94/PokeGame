using AutoMapper;
using PokeGame.Application.Models;
using PokeGame.Web.Models.Users;
using PortalModels = Logitar.Portal.Core;
using UserModels = Logitar.Portal.Core.Users.Models;

namespace PokeGame.Web.Profiles
{
  internal class PortalProfile : Profile
  {
    public PortalProfile()
    {
      CreateMap<PortalModels.Actors.Models.ActorModel, ActorModel>();
      CreateMap<PortalModels.ListModel<UserModels.UserSummary>, ListModel<UserSummary>>();
      CreateMap<UserModels.UserSummary, UserSummary>()
        .ForMember(x => x.PasswordChangedOn, x => x.MapFrom(y => y.PasswordChangedAt))
        .ForMember(x => x.SignedInOn, x => x.MapFrom(y => y.SignedInAt))
        .ForMember(x => x.UpdatedOn, x => x.MapFrom(y => y.UpdatedAt));
    }
  }
}
