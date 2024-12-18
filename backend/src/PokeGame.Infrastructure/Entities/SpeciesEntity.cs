using PokeGame.Domain;
using PokeGame.Domain.Speciez;
using PokeGame.Domain.Speciez.Events;
using PokeGame.Infrastructure.PokeGameDb;

namespace PokeGame.Infrastructure.Entities;

internal class SpeciesEntity : AggregateEntity
{
  public int SpeciesId { get; private set; }
  public Guid Id { get; private set; }

  public int Number { get; private set; }
  public SpeciesCategory Category { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => Helper.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }

  public GrowthRate GrowthRate { get; private set; }
  public byte BaseFriendship { get; private set; }
  public byte CatchRate { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public List<RegionalSpeciesEntity> RegionalSpecies { get; private set; } = [];

  public SpeciesEntity(SpeciesCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Number = @event.Number;
    Category = @event.Category;

    UniqueName = @event.UniqueName.Value;

    GrowthRate = @event.GrowthRate;
    BaseFriendship = @event.BaseFriendship.Value;
    CatchRate = (byte)@event.CatchRate.Value;
  }

  private SpeciesEntity() : base()
  {
  }

  public void SetRegionalNumber(RegionEntity region, SpeciesRegionalNumberChanged @event)
  {
    base.Update(@event);

    RegionalSpeciesEntity? regionalSpecies = RegionalSpecies.SingleOrDefault(x => x.RegionId == region.RegionId);
    if (regionalSpecies == null)
    {
      if (@event.Number.HasValue)
      {
        regionalSpecies = new(this, region, @event);
        RegionalSpecies.Add(regionalSpecies);
      }
    }
    else if (@event.Number.HasValue)
    {
      regionalSpecies.Update(@event);
    }
    else
    {
      RegionalSpecies.Remove(regionalSpecies);
    }
  }

  public void Update(SpeciesUpdated @event)
  {
    base.Update(@event);

    if (@event.UniqueName != null)
    {
      UniqueName = @event.UniqueName.Value;
    }
    if (@event.DisplayName != null)
    {
      DisplayName = @event.DisplayName.Value?.Value;
    }

    if (@event.GrowthRate.HasValue)
    {
      GrowthRate = @event.GrowthRate.Value;
    }
    if (@event.BaseFriendship != null)
    {
      BaseFriendship = @event.BaseFriendship.Value;
    }
    if (@event.CatchRate != null)
    {
      CatchRate = (byte)@event.CatchRate.Value;
    }

    if (@event.Link != null)
    {
      Link = @event.Link.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
