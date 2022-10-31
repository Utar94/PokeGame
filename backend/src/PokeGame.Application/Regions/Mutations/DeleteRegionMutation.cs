using MediatR;

namespace PokeGame.Application.Regions.Mutations
{
  public class DeleteRegionMutation : IRequest
  {
    public DeleteRegionMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
