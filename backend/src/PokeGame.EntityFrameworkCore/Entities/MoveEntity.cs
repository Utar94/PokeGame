using PokeGame.Contracts;
using PokeGame.Contracts.Moves;
using PokeGame.Domain.Moves.Events;

namespace PokeGame.EntityFrameworkCore.Entities;

internal class MoveEntity : AggregateEntity
{
  public int MoveId { get; private set; }

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => PokemonDb.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public byte? Accuracy { get; private set; }
  public byte? Power { get; private set; }
  public byte PowerPoints { get; private set; }

  public Dictionary<string, int> StatisticChanges { get; private set; } = [];
  public string? StatisticChangesSerialized
  {
    get => StatisticChanges.Count == 0 ? null : JsonSerializer.Serialize(StatisticChanges);
    private set
    {
      if (value == null)
      {
        StatisticChanges.Clear();
      }
      else
      {
        StatisticChanges = JsonSerializer.Deserialize<Dictionary<string, int>>(value) ?? [];
      }
    }
  }
  public Dictionary<string, int> StatusConditions { get; private set; } = [];
  public string? StatusConditionsSerialized
  {
    get => StatusConditions.Count == 0 ? null : JsonSerializer.Serialize(StatusConditions);
    private set
    {
      if (value == null)
      {
        StatusConditions.Clear();
      }
      else
      {
        StatusConditions = JsonSerializer.Deserialize<Dictionary<string, int>>(value) ?? [];
      }
    }
  }

  public string? Reference { get; private set; }
  public string? Notes { get; private set; }

  public MoveEntity(MoveCreatedEvent @event) : base(@event)
  {
    Type = @event.Type;
    Category = @event.Category;

    UniqueName = @event.UniqueName.Value;

    PowerPoints = (byte)@event.PowerPoints;
  }

  private MoveEntity() : base()
  {
  }

  public void Update(MoveUpdatedEvent @event)
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
    if (@event.Description != null)
    {
      Description = @event.Description.Value?.Value;
    }

    if (@event.Accuracy != null)
    {
      Accuracy = (byte?)@event.Accuracy.Value;
    }
    if (@event.Power != null)
    {
      Power = (byte?)@event.Power.Value;
    }
    if (@event.PowerPoints.HasValue)
    {
      PowerPoints = (byte)@event.PowerPoints.Value;
    }

    foreach (KeyValuePair<string, int> statisticChange in @event.StatisticChanges)
    {
      if (statisticChange.Value == 0)
      {
        StatisticChanges.Remove(statisticChange.Key);
      }
      else
      {
        StatisticChanges[statisticChange.Key] = statisticChange.Value;
      }
    }
    foreach (KeyValuePair<string, int> statusCondition in @event.StatusConditions)
    {
      if (statusCondition.Value == 0)
      {
        StatusConditions.Remove(statusCondition.Key);
      }
      else
      {
        StatusConditions[statusCondition.Key] = statusCondition.Value;
      }
    }

    if (@event.Reference != null)
    {
      Reference = @event.Reference.Value?.Value;
    }
    if (@event.Notes != null)
    {
      Notes = @event.Notes.Value?.Value;
    }
  }
}
