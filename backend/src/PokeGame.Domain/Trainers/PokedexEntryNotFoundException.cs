using System.Text;

namespace PokeGame.Domain.Trainers
{
  public class PokedexEntryNotFoundException : Exception
  {
    public PokedexEntryNotFoundException(Trainer trainer, Guid speciesId)
       : base(GetMessage(trainer, speciesId))
    {
      Data["TrainerId"] = trainer?.Id ?? throw new ArgumentNullException(nameof(trainer));
      Data["SpeciesId"] = speciesId;
    }

    private static string GetMessage(Trainer trainer, Guid speciesId)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokédex entry could not be found.");
      message.AppendLine($"Trainer: {trainer}");
      message.AppendLine($"SpeciesId: {speciesId}");

      return message.ToString();
    }
  }
}
