using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Models.Api.Game
{
  public class GameTrainerModel
  {
    public GameTrainerModel(TrainerModel model)
    {
      ArgumentNullException.ThrowIfNull(model);

      Id = model.Id;

      Region = model.Region;
      Number = model.Number;

      Gender = model.Gender;
      Name = model.Name;

      Money = model.Money;

      Pokedex = model.PokedexCount;
      HasNationalPokedex = model.NationalPokedex;

      PlayTime = model.PlayTime;
      AdventureStarted = model.CreatedAt;

      Picture = model.Picture;
    }

    public Guid Id { get; set; }

    public Region Region { get; set; }
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; }

    public int Money { get; set; }

    public long Pokedex { get; set; }
    public bool HasNationalPokedex { get; set; }

    public int PlayTime { get; set; }
    public DateTime AdventureStarted { get; set; }

    public string? Picture { get; set; }
  }
}
