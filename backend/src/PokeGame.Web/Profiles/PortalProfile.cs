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
      CreateMap<UserModels.UserSummary, UserSummary>();
    }
  }
}
