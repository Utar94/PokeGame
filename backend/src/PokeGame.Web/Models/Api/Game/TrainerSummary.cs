using PokeGame.Application.Trainers.Models;

namespace PokeGame.Web.Models.Api.Game
{
  public class TrainerSummary
  {
    public TrainerSummary(TrainerModel trainer)
    {
      Number = trainer.Number;
      Name = trainer.Name;

      Picture = trainer.Picture;
    }

    public int Number { get; set; }
    public string Name { get; set; }

    public string? Picture { get; set; }
  }
}
