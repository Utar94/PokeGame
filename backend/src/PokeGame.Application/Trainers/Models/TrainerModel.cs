using PokeGame.Application.Models;
using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Application.Trainers.Models
{
  public class TrainerModel : AggregateModel
  {
    public Guid? UserId { get; set; }

    public Region Region { get; set; }
    public int Number { get; set; }
    public byte Checksum { get; set; }

    public int Money { get; set; }

    public TrainerGender Gender { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string? Notes { get; set; }
    public string? Reference { get; set; }
  }
}
