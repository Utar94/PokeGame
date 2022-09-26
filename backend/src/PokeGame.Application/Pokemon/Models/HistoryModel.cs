using PokeGame.Application.Items.Models;
using PokeGame.Application.Trainers.Models;

namespace PokeGame.Application.Pokemon.Models
{
  public class HistoryModel
  {
    public ItemModel? Ball { get; set; }
    public byte Level { get; set; }
    public string Location { get; set; } = string.Empty;
    public DateTime MetOn { get; set; }
    public TrainerModel? Trainer { get; set; }
  }
}
