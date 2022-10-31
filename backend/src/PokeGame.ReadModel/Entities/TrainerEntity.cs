using PokeGame.Domain;
using PokeGame.Domain.Trainers;

namespace PokeGame.ReadModel.Entities
{
  internal class TrainerEntity : Entity
  {
    public UserEntity? User { get; private set; }
    public int? UserId { get; private set; }

    [Obsolete("To be replaced by the Region entity.")]
    public Region LegacyRegion { get; private set; }

    public RegionEntity? Region { get; private set; }
    public int? RegionId { get; private set; }
    public int Number { get; private set; }

    public int Money { get; private set; }
    public int PlayTime { get; private set; }

    public TrainerGender Gender { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }

    public string? Notes { get; private set; }
    public string? Picture { get; private set; }
    public string? Reference { get; private set; }

    public bool NationalPokedex { get; private set; }
    public int PokedexCount { get; private set; }

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

    public void SetRegion(RegionEntity? region)
    {
      Region = region;
      RegionId = region?.Sid;
    }

    public void SetUser(UserEntity? user)
    {
      User = user;
      UserId = user?.Sid;
    }

    public void Synchronize(Trainer trainer)
    {
      base.Synchronize(trainer);

      Number = trainer.Number;

      Money = trainer.Money;
      PlayTime = trainer.PlayTime;

      Gender = trainer.Gender;
      Name = trainer.Name;
      Description = trainer.Description;

      Notes = trainer.Notes;
      Picture = trainer.Picture;
      Reference = trainer.Reference;
    }

    public void UpdatePokedex(int regionalCount)
    {
      int currentCount = Pokedex.Where(x => x.Species != null)
        .Count(x => x.Species!.RegionalSpecies.Any(y => y.Region == LegacyRegion));
      NationalPokedex = currentCount == regionalCount;

      PokedexCount = NationalPokedex ? Pokedex.Count : currentCount;
    }
  }
}
