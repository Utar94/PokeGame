using AutoMapper;
using PokeGame.Application.Inventories.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class InventoryProfile : Profile
  {
    public InventoryProfile()
    {
      CreateMap<Inventory, InventoryModel>();
    }
  }
}
