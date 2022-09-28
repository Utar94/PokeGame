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
      CreatedOn = model.CreatedAt;

      Region = model.Region;
      Number = model.Number;

      Money = model.Money;

      Gender = model.Gender;
      Name = model.Name;

      Picture = model.Picture;
    }

    public Guid Id { get; set; }
    public DateTime CreatedOn { get; set; }

    public Region Region { get; set; }
    public int Number { get; set; }

    public int Money { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; }

    public string? Picture { get; set; }
  }
}
