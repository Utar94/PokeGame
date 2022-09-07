using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class InvalidAbilityException : Exception
  {
    public InvalidAbilityException(Domain.Species.Species species, Guid abilityId)
      : base(GetMessage(species, abilityId))
    {
      Data["Species"] = species ?? throw new ArgumentNullException(nameof(species));
      Data["AbilityId"] = abilityId;
    }

    private static string GetMessage(Domain.Species.Species species, Guid abilityId)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified ability is not a species ability.");
      message.AppendLine($"Species: {species}");
      message.AppendLine($"AbilityId: {abilityId}");

      return message.ToString();
    }
  }
}
