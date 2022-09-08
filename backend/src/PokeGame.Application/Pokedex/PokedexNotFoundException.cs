using System.Text;

namespace PokeGame.Application.Pokedex
{
  public class PokedexNotFoundException : Exception
  {
    public PokedexNotFoundException(Guid trainerId, Guid speciesId)
      : base(GetMessage(trainerId, speciesId))
    {
      Data["TrainerId"] = trainerId;
      Data["Species"] = speciesId;
    }

    private static string GetMessage(Guid trainerId, Guid speciesId)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokedex could not be found.");
      message.AppendLine($"TrainerId: {trainerId}");
      message.AppendLine($"SpeciesId: {speciesId}");

      return message.ToString();
    }
  }
}
