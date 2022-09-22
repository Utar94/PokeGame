using MediatR;

namespace PokeGame.Application.Species.Mutations
{
  public class RemoveSpeciesEvolutionMutation : IRequest
  {
    public RemoveSpeciesEvolutionMutation(Guid id, Guid speciesId)
    {
      Id = id;
      SpeciesId = speciesId;
    }

    public Guid Id { get; }
    public Guid SpeciesId { get; }
  }
}
