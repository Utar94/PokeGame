using PokeGame.Contracts.Species;
using PokeGame.Domain.Species;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class PokemonSpeciesEntity : AggregateEntity
{
  public int PokemonSpeciesId { get; private set; }
  public Guid Id { get; private set; }

  public int Number { get; private set; }
  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => PokeGameDb.Helper.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public PokemonCategory? Category { get; private set; }

  public int BaseHappiness { get; private set; }
  public int CaptureRate { get; private set; }
  public LevelingRate LevelingRate { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  // TODO(fpion): PokedexNumbers

  public PokemonSpeciesEntity(PokemonSpecies.CreatedEvent @event) : base(@event)
  {
    Id = @event.AggregateId.ToGuid();

    Number = @event.Number;
    UniqueName = @event.UniqueName.Value;

    LevelingRate = @event.LevelingRate;
  }

  private PokemonSpeciesEntity() : base()
  {
  }

  public void Update(PokemonSpecies.UpdatedEvent @event)
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
    if (@event.Category != null)
    {
      Category = @event.Category.Value;
    }

    if (@event.BaseHappiness.HasValue)
    {
      BaseHappiness = @event.BaseHappiness.Value;
    }
    if (@event.CaptureRate.HasValue)
    {
      CaptureRate = @event.CaptureRate.Value;
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
