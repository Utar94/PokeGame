using PokeGame.Application.Moves.Models;
using PokeGame.Domain;
using PokeGame.Domain.Moves;
using PokeGame.Domain.Moves.Events;
using PokeGame.Infrastructure.Converters;
using PokeGame.Infrastructure.PokeGameDb;

namespace PokeGame.Infrastructure.Entities;

internal class MoveEntity : AggregateEntity
{
  private static readonly JsonSerializerOptions _serializerOptions = new();
  static MoveEntity()
  {
    _serializerOptions.Converters.Add(new JsonStringEnumConverter());
    _serializerOptions.Converters.Add(new VolatileConditionConverter());
  }

  public int MoveId { get; private set; }
  public Guid Id { get; private set; }

  public PokemonType Type { get; private set; }
  public MoveCategory Category { get; private set; }

  public string UniqueName { get; private set; } = string.Empty;
  public string UniqueNameNormalized
  {
    get => Helper.Normalize(UniqueName);
    private set { }
  }
  public string? DisplayName { get; private set; }
  public string? Description { get; private set; }

  public byte? Accuracy { get; private set; }
  public byte? Power { get; private set; }
  public byte PowerPoints { get; private set; }

  public StatusCondition? StatusCondition { get; private set; }
  public int? StatusChance { get; private set; }
  public string? StatisticChanges { get; private set; }
  public string? VolatileConditions { get; private set; }

  public string? Link { get; private set; }
  public string? Notes { get; private set; }

  public MoveEntity(MoveCreated @event) : base(@event)
  {
    Id = @event.StreamId.ToGuid();

    Type = @event.Type;
    Category = @event.Category;

    UniqueName = @event.UniqueName.Value;
  }

  private MoveEntity() : base()
  {
  }

  public void Update(MoveUpdated @event)
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
      Accuracy = (byte?)@event.Accuracy.Value?.Value;
    }
    if (@event.Power != null)
    {
      Power = (byte?)@event.Power.Value?.Value;
    }
    if (@event.PowerPoints != null)
    {
      PowerPoints = (byte)@event.PowerPoints.Value;
    }

    if (@event.InflictedStatus != null)
    {
      StatusCondition = @event.InflictedStatus.Value?.Condition;
      StatusChance = @event.InflictedStatus.Value?.Chance;
    }

    Dictionary<PokemonStatistic, int> statisticChanges = GetStatisticChanges();
    foreach (KeyValuePair<PokemonStatistic, int> statisticChange in @event.StatisticChanges)
    {
      if (statisticChange.Value == 0)
      {
        statisticChanges.Remove(statisticChange.Key);
      }
      else
      {
        statisticChanges[statisticChange.Key] = statisticChange.Value;
      }
    }
    SetStatisticChanges(statisticChanges);

    if (@event.VolatileConditions != null)
    {
      SetVolatileConditions(@event.VolatileConditions);
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

  public InflictedStatusModel? GetInflictedStatus()
  {
    return StatusCondition.HasValue && StatusChance.HasValue ? new(StatusCondition.Value, StatusChance.Value) : null;
  }

  public Dictionary<PokemonStatistic, int> GetStatisticChanges()
  {
    return (StatisticChanges == null ? null : JsonSerializer.Deserialize<Dictionary<PokemonStatistic, int>>(StatisticChanges, _serializerOptions)) ?? [];
  }
  private void SetStatisticChanges(Dictionary<PokemonStatistic, int> statisticChanges)
  {
    StatisticChanges = statisticChanges.Count < 1 ? null : JsonSerializer.Serialize(statisticChanges, _serializerOptions);
  }

  public IReadOnlyCollection<VolatileCondition> GetVolatileConditions()
  {
    return (VolatileConditions == null ? null : JsonSerializer.Deserialize<IReadOnlyCollection<VolatileCondition>>(VolatileConditions, _serializerOptions)) ?? [];
  }
  private void SetVolatileConditions(IReadOnlyCollection<VolatileCondition> volatileConditions)
  {
    VolatileConditions = volatileConditions.Count < 1 ? null : JsonSerializer.Serialize(volatileConditions, _serializerOptions);
  }

  public override string ToString() => $"{DisplayName ?? UniqueName} | {base.ToString()}";
}
