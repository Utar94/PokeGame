using AutoMapper;
using PokeGame.Core.Inventories.Models;

namespace PokeGame.Core.Inventories
{
  internal class InventoryProfile : Profile
  {
    public InventoryProfile()
    {
      CreateMap<Inventory, InventoryModel>();
    }
  }
}
