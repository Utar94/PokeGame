using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class TrainerEntity : Entity
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

    public List<InventoryEntity> Inventory { get; set; } = new();
    public List<PokemonEntity> OriginalPokemon { get; set; } = new();
    public List<PokemonEntity> Pokemon { get; set; } = new();

    public void Synchronize(Trainer trainer)
    {
      UserId = trainer.UserId;

      Region = trainer.Region;
      Number = trainer.Number;
      Checksum = trainer.Checksum;

      Money = trainer.Money;

      Gender = trainer.Gender;
      Name = trainer.Name;
      Description = trainer.Description;

      Notes = trainer.Notes;
      Reference = trainer.Reference;
    }
  }
}
