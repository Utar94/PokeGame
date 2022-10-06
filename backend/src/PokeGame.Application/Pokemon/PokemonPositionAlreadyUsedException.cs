using PokeGame.Domain.Pokemon;
using PokeGame.Domain.Trainers;
using System.Text;

namespace PokeGame.Application.Pokemon
{
  public class PokemonPositionAlreadyUsedException : Exception
  {
    public PokemonPositionAlreadyUsedException(Trainer trainer, PokemonPosition position)
      : base(GetMessage(trainer, position))
    {
      Data["TrainerId"] = trainer.Id;
      Data["Box"] = position.Box;
      Data["Position"] = position.Position;
    }

    private static string GetMessage(Trainer trainer, PokemonPosition position)
    {
      var message = new StringBuilder();

      message.AppendLine("The specified Pokémon position is already used for the specified trainer.");
      message.AppendLine($"Trainer: {trainer}");
      message.AppendLine($"Position: {position}");

      return message.ToString();
    }
  }
}
