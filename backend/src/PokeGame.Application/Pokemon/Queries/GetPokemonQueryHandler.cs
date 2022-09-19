using MediatR;
using PokeGame.Application.Pokemon.Models;

namespace PokeGame.Application.Pokemon.Queries
{
  internal class GetPokemonQueryHandler : IRequestHandler<GetPokemonQuery, PokemonModel?>
  {
    private readonly IPokemonQuerier _querier;

    public GetPokemonQueryHandler(IPokemonQuerier querier)
    {
      _querier = querier;
    }

    public async Task<PokemonModel?> Handle(GetPokemonQuery request, CancellationToken cancellationToken)
    {
      return await _querier.GetAsync(request.Id, cancellationToken);
    }
  }
}
