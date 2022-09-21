using PokeGame.Domain.Trainers;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokedexEntity
  {
    public PokedexEntity(TrainerEntity trainer, SpeciesEntity species)
    {
      Trainer = trainer ?? throw new ArgumentNullException(nameof(trainer));
      TrainerId = trainer.Sid;
      Species = species ?? throw new ArgumentNullException(nameof(species));
      SpeciesId = species.Sid;
    }
    private PokedexEntity()
    {
    }

    public TrainerEntity? Trainer { get; private set; }
    public int TrainerId { get; private set; }

    public SpeciesEntity? Species { get; private set; }
    public int SpeciesId { get; private set; }

    public bool HasCaught { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public void Synchronize(PokedexEntry entry)
    {
      HasCaught = entry.HasCaught;
      UpdatedAt = entry.UpdatedAt;
    }
  }
}
