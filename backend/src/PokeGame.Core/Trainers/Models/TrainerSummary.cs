using PokeGame.Core.Models;

namespace PokeGame.Core.Trainers.Models
{
  public class TrainerSummary : AggregateSummary
  {
    //public Guid? UserId { get; set; } // TODO(fpion): implement

    public Region Region { get; set; }
    public int Number { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; } = null!;
  }
}
