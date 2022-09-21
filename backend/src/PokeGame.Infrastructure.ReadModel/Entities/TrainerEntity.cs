using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.Infrastructure.ReadModel.Entities
{
  internal class TrainerEntity : Entity
  {
    public Guid? UserId { get; private set; }

    public Region Region { get; private set; }
    public int Number { get; private set; }
    public byte Checksum { get; private set; }

    public int Money { get; private set; }

    public TrainerGender Gender { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public List<InventoryEntity> Inventory { get; private set; } = new();
    public List<PokemonEntity> OriginalPokemon { get; private set; } = new();
    public List<PokedexEntity> Pokedex { get; private set; } = new();
    public List<PokemonEntity> Pokemon { get; private set; } = new();
    public List<PokemonPositionEntity> PokemonPositions { get; private set; } = new();

    public InventoryEntity Add(ItemEntity item)
    {
      var entity = new InventoryEntity(this, item);

      Inventory.Add(entity);

      return entity;
    }

    public PokedexEntity AddPokedex(SpeciesEntity species)
    {
      var entity = new PokedexEntity(this, species);

      Pokedex.Add(entity);

      return entity;
    }

    public void Synchronize(Trainer trainer)
    {
      base.Synchronize(trainer);

      UserId = trainer.UserId;

      Region = trainer.Region;
      Number = trainer.Number;
      Checksum = trainer.Checksum;

      Money = trainer.Money;

      Gender = trainer.Gender;
      Name = trainer.Name;
      Description = trainer.Description;

      Notes = trainer.Notes;
      Picture = trainer.Picture;
      Reference = trainer.Reference;
    }
  }
}
