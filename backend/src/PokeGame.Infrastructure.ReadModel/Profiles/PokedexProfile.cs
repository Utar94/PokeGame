using AutoMapper;
using PokeGame.Application.Pokedex.Models;
using PokeGame.Infrastructure.ReadModel.Entities;

namespace PokeGame.Infrastructure.ReadModel.Profiles
{
  internal class PokedexProfile : Profile
  {
    public PokedexProfile()
    {
      CreateMap<PokedexEntity, PokedexModel>();
    }
  }
}
