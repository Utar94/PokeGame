using AutoMapper;
using PokeGame.Application.Pokedex.Models;
using PokeGame.ReadModel.Entities;

namespace PokeGame.ReadModel.Profiles
{
  internal class PokedexProfile : Profile
  {
    public PokedexProfile()
    {
      CreateMap<PokedexEntity, PokedexModel>();
    }
  }
}
