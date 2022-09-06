using AutoMapper;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class ItemProfile : Profile
  {
    public ItemProfile()
    {
      CreateMap<Item, ItemModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
