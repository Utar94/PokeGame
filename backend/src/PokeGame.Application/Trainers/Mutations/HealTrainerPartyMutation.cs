using MediatR;

namespace PokeGame.Application.Trainers.Mutations
{
  public class HealTrainerPartyMutation : IRequest
  {
    public HealTrainerPartyMutation(Guid id)
    {
      Id = id;
    }

    public Guid Id { get; }
  }
}
