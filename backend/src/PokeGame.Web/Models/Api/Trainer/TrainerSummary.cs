using PokeGame.Application.Trainers.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Web.Models.Api.Trainer
{
  public class TrainerSummary : AggregateSummary
  {
    public TrainerSummary(TrainerModel model) : base(model)
    {
      Region = model.Region;
      Number = model.Number;

      Gender = model.Gender;
      Name = model.Name;
    }

    public Region Region { get; set; }
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; } = string.Empty;
  }
}
