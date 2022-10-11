using AutoMapper;
using PokeGame.Application.Items.Models;
using PokeGame.Application.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class ItemProfile : Profile
  {
    public ItemProfile()
    {
      CreateMap<ItemEntity, ItemModel>()
        .IncludeBase<Entity, AggregateModel>();
    }
  }
}
