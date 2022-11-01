using PokeGame.Application.Trainers.Models;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Models.Api.Game
{
  public class GameTrainerModel
  {
    public GameTrainerModel(TrainerModel model)
    {
      Id = model.Id;

      Region = model.Region?.Name;
      Number = model.Number;

      Gender = model.Gender;
      Name = model.Name;

      Money = model.Money;

      Pokedex = model.PokedexCount;

      PlayTime = model.PlayTime;
      AdventureStarted = model.CreatedOn;

      Picture = model.Picture;
    }

    public Guid Id { get; set; }

    public string? Region { get; set; }
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; }

    public int Money { get; set; }

    public long Pokedex { get; set; }

    public int PlayTime { get; set; }
    public DateTime AdventureStarted { get; set; }

    public string? Picture { get; set; }
  }
}
