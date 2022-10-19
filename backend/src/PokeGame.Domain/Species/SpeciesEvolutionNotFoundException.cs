using System.Text;

namespace PokeGame.Domain.Species
{
  public class SpeciesEvolutionNotFoundException : Exception
  {
    public SpeciesEvolutionNotFoundException(Guid evolvingSpeciesId, Guid evolvedSpeciesId)
      : base(GetMessage(evolvingSpeciesId, evolvedSpeciesId))
    {
      Data["EvolvingSpeciesId"] = evolvingSpeciesId;
      Data["EvolvedSpeciesId"] = evolvedSpeciesId;
    }

    private static string GetMessage(Guid evolvingSpeciesId, Guid evolvedSpeciesId)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified evolution could not be found.");
      message.AppendLine($"Evolving species ID: {evolvingSpeciesId}");
      message.AppendLine($"Evolved species ID: {evolvedSpeciesId}");

      return message.ToString();
    }
  }
}
