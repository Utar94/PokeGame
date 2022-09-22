using MediatR;

namespace PokeGame.Application.Species.Mutations
{
  public class DeleteSpeciesMutation : IRequest
  {
    public DeleteSpeciesMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
