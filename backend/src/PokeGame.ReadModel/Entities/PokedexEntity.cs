using PokeGame.Domain.Trainers;

namespace PokeGame.ReadModel.Entities
{
  internal class PokedexEntity
  {
    public PokedexEntity(TrainerEntity trainer, SpeciesEntity species)
    {
      Trainer = trainer;
      TrainerId = trainer.Sid;
      Species = species;
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
    public DateTime UpdatedOn { get; private set; }

    public void Synchronize(PokedexEntry entry)
    {
      HasCaught = entry.HasCaught;
      UpdatedOn = entry.UpdatedOn;
    }
  }
}
