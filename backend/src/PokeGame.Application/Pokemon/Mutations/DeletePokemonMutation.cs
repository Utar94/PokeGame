using MediatR;

namespace PokeGame.Application.Pokemon.Mutations
{
  public class DeletePokemonMutation : IRequest
  {
    public DeletePokemonMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
