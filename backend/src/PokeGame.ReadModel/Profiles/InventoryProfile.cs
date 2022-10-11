using AutoMapper;
using PokeGame.Application.Inventories.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class InventoryProfile : Profile
  {
    public InventoryProfile()
    {
      CreateMap<InventoryEntity, InventoryModel>();
    }
  }
}
