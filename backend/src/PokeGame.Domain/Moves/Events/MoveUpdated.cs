using Logitar.EventSourcing;
using MediatR;

namespace PokeGame.Domain.Moves.Events;

public record MoveUpdated : DomainEvent, INotification
{
  public UniqueName? UniqueName { get; set; }
  public Change<DisplayName>? DisplayName { get; set; }
  public Change<Description>? Description { get; set; }

  public Change<Accuracy>? Accuracy { get; set; }
  public Change<Power>? Power { get; set; }
  public PowerPoints? PowerPoints { get; set; }

  public Change<InflictedStatus>? InflictedStatus { get; set; }
  public Dictionary<PokemonStatistic, int> StatisticChanges { get; set; } = [];
  public IReadOnlyCollection<VolatileCondition>? VolatileConditions { get; set; }

  public Change<Url>? Link { get; set; }
  public Change<Notes>? Notes { get; set; }

  [JsonIgnore]
  public bool HasChanges => UniqueName != null || DisplayName != null || Description != null
    || Accuracy != null || Power != null || PowerPoints != null
    || InflictedStatus != null || StatisticChanges.Count > 0 || VolatileConditions != null
    || Link != null || Notes != null;
}
