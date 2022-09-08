namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class PokedexEntity
  {
    public TrainerEntity? Trainer { get; set; }
    public int TrainerId { get; set; }

    public SpeciesEntity? Species { get; set; }
    public int SpeciesId { get; set; }

    public bool HasCaught { get; set; }
    public DateTime UpdatedAt { get; set; }
  }
}
